using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Wikimedia.Utilities.Dtos;
using Wikimedia.Utilities.ExtensionMethods;
using Wikimedia.Utilities.Interfaces;
using Wikimedia.Utilities.Models;
using Wikimedia.Utilities.Services;
using WikipediaDeathsPages.Data.Interfaces;
using WikipediaDeathsPages.Service;
using WikipediaDeathsPages.Service.Interfaces;
using Xunit;

namespace WikipediaDeathsPagesTests
{
    public class WikipediaServiceShould
    {
        private readonly ILogger<WikipediaService> logger;
        private readonly Mock<IWikidataService> wikidataServiceMock;
        private readonly Mock<IWikipediaWebClient> webClientMock;
        private readonly Mock<IReferenceService> referenceServiceMock;
        private readonly Mock<IToolforgeService> toolforgeServiceMock;

        public WikipediaServiceShould()
        {
            logger = new NullLogger<WikipediaService>();
            wikidataServiceMock = new Mock<IWikidataService>();
            webClientMock = new Mock<IWikipediaWebClient>();
            referenceServiceMock = new Mock<IReferenceService>();
            var articleName = CreateWikidataItemDto(description: null).ArticleName;
            toolforgeServiceMock = new Mock<IToolforgeService>();
            toolforgeServiceMock.Setup(_ => _.GetWikilinksInfo(articleName)).Returns(new Wikilinks { all = 600, direct = 500, indirect = 100 });
        }

        [Fact(DisplayName = "Get the deceased belonging to a date of death")]
        public void GetEntriesPerDeathDate()
        {
            var deathDate = DateTime.Today;
            string month = deathDate.ToString("MMMM", new CultureInfo("en-US"));
            var sanitizedDateOfDeathReferences = "xFaG~!enwiki";
            var itemDto = CreateWikidataItemDto(description: "American singer, actress and model");

            referenceServiceMock.Setup(_ => _.Resolve(deathDate, sanitizedDateOfDeathReferences, itemDto.Label, null)).Returns("");

            wikidataServiceMock.Setup(_ => _.GetItemsPerDeathDate(deathDate, false)).Returns(new List<WikidataItemDto> { itemDto });
            wikidataServiceMock.Setup(_ => _.ResolveDateOfBirth(itemDto)).Returns((DateTime)itemDto.DateOfBirth);
            wikidataServiceMock.Setup(_ => _.ResolveBiolink(itemDto)).Returns($"[[{itemDto.ArticleName}]]");
            wikidataServiceMock.Setup(_ => _.ResolveItemDescription(itemDto)).Returns(itemDto.Description);
            wikidataServiceMock.Setup(_ => _.ResolveItemCauseOfDeath(itemDto)).Returns(itemDto.CauseOfDeath);
            wikidataServiceMock.Setup(_ => _.SanitizeDateOfDeathReferences(itemDto)).Returns(sanitizedDateOfDeathReferences);
            wikidataServiceMock.Setup(_ => _.ResolveAge(itemDto)).Returns("22, ");

            string s;
            var returnWikiTextMonthArticle =
                $"=={month} {deathDate.Year}==\n" +
                $"==={deathDate.Day}===\n" +
                 "==References==";

            webClientMock.Setup(_ => _.GetWikiTextArticle(itemDto.ArticleName, out s)).Returns("infobox{{ | Death_cause   = car crash  | .. | ..}} She was an American singer, actress and model. She was born..");
            webClientMock.Setup(_ => _.GetWikiTextArticle($"Deaths in {month} {deathDate.Year}", out s)).Returns(returnWikiTextMonthArticle);

            var wikipediaService = new WikipediaService(wikidataServiceMock.Object, referenceServiceMock.Object,
                                                        new WikiTextService(), toolforgeServiceMock.Object, webClientMock.Object, logger);

            var result = wikipediaService.GetDeathDateResult(deathDate, 48);

            Assert.Equal("*[[Aaliyah]], 22, American singer, actress and model, aviation accident.", result.Entries.First().WikiText);
        }

        [Fact(DisplayName ="Resolve the date of death anomalie")]
        public void ResolveDeathDateAnomalie()
        {
            var deathDate = new DateTime(1999, 4, 1);
            var wikiTextServiceMock = new Mock<IWikiTextService>();

            string s;
            var dummyWikiText = "boeit niet";
            var articleLinkedName = "Linked Name";
            var category = $"[[Category:{deathDate.Year} deaths]]"; 
            var articleText = $"{articleLinkedName} (1900 &ndash; {deathDate.Year}) was a Dutch athlete. {category}";

            wikiTextServiceMock.Setup(_ => _.GetWikiTextDeathsPerMonth(deathDate, false, null)).Returns(dummyWikiText);
            wikiTextServiceMock.Setup(_ => _.GetDaySectionOfMonthList(dummyWikiText, 1)).Returns(dummyWikiText);
            wikiTextServiceMock.Setup(_ => _.GetDeceasedTextAsList(dummyWikiText)).Returns(new string[] { $"[[{articleLinkedName}|Name]], 99, description, cause of death." });
            wikiTextServiceMock.Setup(_ => _.ResolveDate(articleText, deathDate)).Returns(DateTime.MinValue);
            webClientMock.Setup(_ => _.GetWikiTextArticle(articleLinkedName, out s)).Returns(articleText);

            var wikipediaService = new WikipediaService(wikidataServiceMock.Object, referenceServiceMock.Object,
            wikiTextServiceMock.Object, toolforgeServiceMock.Object, webClientMock.Object, logger);

            var anomalies = wikipediaService.ResolveArticleAnomalies(deathDate.Year, deathDate.Month);
            Assert.Equal("Death date 1 April 1999 not encountered in article", anomalies.First().Text);
        }

        [Fact(DisplayName = "Resolve the category anomalie")]
        public void ResolveCategorieAnomalie()
        {
            var deathDate = new DateTime(1999, 4, 1);
            var wikiTextServiceMock = new Mock<IWikiTextService>();

            string s;
            var dummyWikiText = "boeit niet";
            var articleLinkedName = "Linked Name";
            var deathDateText = deathDate.ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("en-US")); // not necessary for test btw
            var articleText = $"{articleLinkedName} (1 January 1900 &ndash; {deathDateText}) was a Dutch athlete.";

            wikiTextServiceMock.Setup(_ => _.GetWikiTextDeathsPerMonth(deathDate, false, null)).Returns(dummyWikiText);
            wikiTextServiceMock.Setup(_ => _.GetDaySectionOfMonthList(dummyWikiText, 1)).Returns(dummyWikiText);
            wikiTextServiceMock.Setup(_ => _.GetDeceasedTextAsList(dummyWikiText)).Returns(new string[] { $"[[{articleLinkedName}|Name]], 99, description, cause of death." });
            wikiTextServiceMock.Setup(_ => _.ResolveDate(articleText, deathDate)).Returns(deathDate);
            webClientMock.Setup(_ => _.GetWikiTextArticle(articleLinkedName, out s)).Returns(articleText);

            var wikipediaService = new WikipediaService(wikidataServiceMock.Object, referenceServiceMock.Object,
            wikiTextServiceMock.Object, toolforgeServiceMock.Object, webClientMock.Object, logger);

            var anomalies = wikipediaService.ResolveArticleAnomalies(deathDate.Year, deathDate.Month);
            Assert.Equal("Categorie '[[Category:1999 deaths' not encountered in article", anomalies.First().Text);
        }

        [Fact(DisplayName = "Resolve multiple anomalies")]
        public void ResolveMultipleAnomalies()
        {
            var deathDate = new DateTime(1999, 4, 1);
            var wikiTextServiceMock = new Mock<IWikiTextService>();

            string s;
            var dummyWikiText = "boeit niet";
            var articleLinkedName = "Linked Name";
            var articleText = $"{articleLinkedName} (1900 &ndash; {deathDate.Year}) was a Dutch athlete.";

            wikiTextServiceMock.Setup(_ => _.GetWikiTextDeathsPerMonth(deathDate, false, null)).Returns(dummyWikiText);
            wikiTextServiceMock.Setup(_ => _.GetDaySectionOfMonthList(dummyWikiText, 1)).Returns(dummyWikiText);
            wikiTextServiceMock.Setup(_ => _.GetDeceasedTextAsList(dummyWikiText)).Returns(new string[] { $"[[{articleLinkedName}|Name]], 99, description, cause of death." });
            wikiTextServiceMock.Setup(_ => _.ResolveDate(articleText, deathDate)).Returns(DateTime.MinValue);
            webClientMock.Setup(_ => _.GetWikiTextArticle(articleLinkedName, out s)).Returns(articleText);

            var wikipediaService = new WikipediaService(wikidataServiceMock.Object, referenceServiceMock.Object,
            wikiTextServiceMock.Object, toolforgeServiceMock.Object, webClientMock.Object, logger);

            var anomalies = wikipediaService.ResolveArticleAnomalies(deathDate.Year, deathDate.Month);
            Assert.Equal("Death date 1 April 1999 not encountered in article, Categorie '[[Category:1999 deaths' not encountered in article", anomalies.First().Text);
        }

        [Fact(DisplayName = "Resolve Olympic reference")]
        public void HandleExceptionCorrectly()
        {
            string s;
            var itemDto = CreateWikidataItemDto(description: "American singer, actress and model");

            wikidataServiceMock.Setup(_ => _.GetItemsPerDeathDate(DateTime.MinValue, false)).Returns(new List<WikidataItemDto> { itemDto });
            wikidataServiceMock.Setup(_ => _.ResolveItemLabel(itemDto)).Returns(itemDto.ArticleName);
            webClientMock.Setup(_ => _.GetWikiTextArticle(itemDto.ArticleName, out s)).Returns($"{itemDto.ArticleName} was a Dutch athlete who was an Olympic champion.");

            var wikipediaReferencesMock = new Mock<IWikipediaReferences>();
            wikipediaReferencesMock.Setup(_ => _.GetIdsOfName(itemDto.ArticleName)).Throws<InvalidOperationException>();

            var referenceService = new ReferenceService(wikipediaReferencesMock.Object, null, null);
            var wikipediaService = new WikipediaService(wikidataServiceMock.Object, referenceService, new WikiTextService(), toolforgeServiceMock.Object, webClientMock.Object, logger);

            var expectedSubstring = $"error: {itemDto.ArticleName} [WikipediaService.ResolveEntryText]: Operation is not valid due to the current state of the object.";
            var result = wikipediaService.GetDeathDateResult(DateTime.MinValue, 48);

            Assert.Equal(expectedSubstring, result.Entries.First().WikiText);
        }

        [Fact]
        public void TestExtensionMethodValueBetweenTwoStrings()
        {
            const string testValue = "Test value";
            string s = $"\"{testValue}\"";
            string actual = s.ValueBetweenTwoStrings("\"", "\"");

            Assert.Equal(testValue, actual);
        }

        [Fact]
        public void TestMonths()
        {
            var ci = new CultureInfo("en-US");

            DateTime date = new DateTime(1999, 6, 24);

            var dateText = date.ToString("MMM dd, yyyy", ci);

            Assert.Equal("Jun 24, 1999", dateText);
        }

        private WikidataItemDto CreateWikidataItemDto(string description)
        {
            return new WikidataItemDto
            {
                ArticleName = "Aaliyah",
                Label = "Aaliyah ",
                Description = description,
                Uri = "https://www.wikidata.org/wiki/Q11617",
                SiteLinksCount = 60,
                DateOfBirth = DateTime.Parse("16 January 1979"),
                DateOfDeath = DateTime.Parse("25 August 2001"),
                DateOfDeathRefs = "ref 1~!ref 2~!ref 3",
                CauseOfDeath = "aviation accident",
                MannerOfDeath = "accident"
            };
        }
    }
}
