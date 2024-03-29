﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Wikimedia.Utilities.Dtos;
using Wikimedia.Utilities.Exceptions;
using Wikimedia.Utilities.ExtensionMethods;
using Wikimedia.Utilities.Interfaces;
using WikipediaDeathsPages.Service.Dtos;
using WikipediaDeathsPages.Service.Interfaces;
using WikipediaDeathsPages.Service.Models;

namespace WikipediaDeathsPages.Service
{
    public class WikipediaService : IWikipediaService
    {
        private readonly IWikidataService wikidataService;
        private readonly IReferenceService referenceService;
        private readonly IWikiTextService wikiTextService;
        private readonly IWikipediaWebClient wikipediaWebClient;
        private readonly ILogger<WikipediaService> logger;
        private int minimumScore;

        public WikipediaService(IWikidataService wikidataService, IReferenceService referenceService, IWikiTextService wikiTextService,
                                IWikipediaWebClient wikipediaWebClient, ILogger<WikipediaService> logger)
        {
            this.wikidataService = wikidataService;
            this.referenceService = referenceService;
            this.wikiTextService = wikiTextService;
            this.wikipediaWebClient = wikipediaWebClient;
            this.logger = logger;
        }

        public ArticleMetrics GetArticleMetrics(string article)
        {
            var linksToArticleCount = wikipediaWebClient.GetWikimediaSearchDirectLinkCount(article);
            int siteLinks = GetSiteLinksCount(article);

            return new ArticleMetrics
            {
                ArticleName = article,
                LinksToArticleCount = linksToArticleCount,
                SiteLinksCount = siteLinks
            };
        }

        private int GetSiteLinksCount(string article)
        {
            try
            {
                return wikidataService.GetSitelinksResult(article).SiteLinksCount;
            }
            catch (NullReferenceException)
            {

                throw new NullReferenceException($"{article}: Exception retrieving site links. No corresponding Wikidata item?");
            }
            catch(Exception e)
            {
                throw new Exception($"WikipediaService.GetSiteLinksCount('{article}'): error calling WikidataService.GetSitelinksResult('{article}'). Message: {e.Message}", e);
            }
        }

        public IEnumerable<ArticleMetrics> GetArticleMetrics(IEnumerable<string> articles)
        {
            var scores = new List<ArticleMetrics>();

            foreach (var article in articles)
                scores.Add(GetArticleMetrics(article));

            return scores;
        }

        public DeathDateResultDto GetDeathDateResult(DateTime deathDate, int minimumScore)
        {
            this.minimumScore = minimumScore;
            var wikidataItems = wikidataService.GetItemsPerDeathDate(deathDate, false);

            if (minimumScore > 0)
                wikidataItems = wikidataItems.Where(i => i.SiteLinksCount > 0);

            List<WikipediaListItemDto> entries = InitializeEntries(wikidataItems);

            // Get the existing entries in the wp month list. Do that now before the wikidata entries are limited
            var existingEntries = GetExistingEntries(GetWikiText(deathDate), deathDate, true).ToList();

            if (existingEntries.Any())
                HandleExistingEntries(deathDate, entries, existingEntries);

            // Limit collection; we don't want to get the WP article text for all wikidata entries
            entries = entries.Where(e => e.NotabilityScore >= minimumScore || e.KeepExisting).ToList();

            // Add information found in the WP article to the entry
            entries.ForEach(e => ResolveArticleData(e));

            // Based on what we now know about them; remove some entries
            Purge(entries);

            // Generate the wiki text of the entries
            entries.ForEach(e => ResolveEntryWikiText(e, existingEntries, deathDate));

            return new DeathDateResultDto
            {
                DateOfDeath = deathDate,
                Entries = entries,
                ScoreNumberFour = ResolveScoreNumberFour(entries),
                RejectedExistingEntries = existingEntries?.Where(ee => !ee.Keep),
            };
        }

        public IEnumerable<ArticleAnomalieResultDto> ResolveArticleAnomalies(int year, int month)
        {
            var anomaliesPerMonth = new List<ArticleAnomalieResultDto>();            
            string wikiText = GetWikiText(new DateTime(year, month, 1));

            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                var entriesPerDay = GetExistingEntries(wikiText, new DateTime(year, month, day), false);

                foreach (var entry in entriesPerDay)
                {
                    var anomalies = ResolveArticleAnomalies(entry);
                    if (anomalies != null)
                        anomaliesPerMonth.Add(anomalies);
                }

            }
            return anomaliesPerMonth;
        }

        private ArticleAnomalieResultDto ResolveArticleAnomalies(ExistingEntryDto entry)
        {
            ArticleAnomalieResultDto anomalies = null;

            string wikiText = wikipediaWebClient.GetWikiTextArticle(entry.ArticleLinkedName, out string redirectedArticle);

            // Check 1: Redirect?
            if (redirectedArticle != null)
            {
                AddAnomalieText(ref anomalies, entry, $"'{entry.ArticleLinkedName}' results in a redirect to '{redirectedArticle}'. Investigate");
                return anomalies; // no need to investigate wiki text '#REDIRECT [[...'
            }
                
            var dateOfDeath = wikiTextService.ResolveDate(wikiText, entry.DateOfDeath);

            if (dateOfDeath == DateTime.MinValue)
            {
                var deathDateText = entry.DateOfDeath.ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("en-US"));
                AddAnomalieText(ref anomalies, entry, $"Death date {deathDateText} not encountered in article");
            }                
            
            var searchTerm1 = GetCategorieSearchTerm(entry.DateOfDeath.Year, "deaths");
            var searchTerm2 = GetCategorieSearchTerm(entry.DateOfDeath.Year, "suicides");

            var comparisonType = StringComparison.OrdinalIgnoreCase;

            if (!(wikiText.Contains(searchTerm1, comparisonType) || wikiText.Contains(searchTerm2, comparisonType)))
                AddAnomalieText(ref anomalies, entry, $"Categorie '{searchTerm1}' not encountered in article");

            return anomalies;
        }

        private string GetCategorieSearchTerm(int yearOfDeath, string termSuffix)
        {
            return "[[Category:" + yearOfDeath + " " + termSuffix;
        }

        private void AddAnomalieText(ref ArticleAnomalieResultDto anomalies, ExistingEntryDto entry, string text )
        {
            if(anomalies == null)
            {
                anomalies = new ArticleAnomalieResultDto
                {
                    Day = entry.DateOfDeath.Day,
                    ArticleLinkedName = entry.ArticleLinkedName,
                    Uri = entry.Uri,                    
                };
            }

            if (!string.IsNullOrEmpty(anomalies.Text))
                anomalies.Text += ", ";

            anomalies.Text += text;
        }

        private void HandleExistingEntries(DateTime deathDate, List<WikipediaListItemDto> entries, List<ExistingEntryDto> existingEntries)
        {
            // enrich the existing items
            EnrichExistingEntries(existingEntries, entries, deathDate);

            // For each of the entries try to find the corresponding existing entry and determine whether to keep it. Sorting issue prevented!
            entries.ForEach(e => EvaluateExistingEntries(e, existingEntries));
        }

        private void ResolveEntryWikiText(WikipediaListItemDto entry, IEnumerable<ExistingEntryDto> existingEntries, DateTime deathDate)
        {
            try
            {
                ExistingEntryDto existingEntry;
                string bioLink = wikidataService.ResolveBiolink(entry.WikidataItem);
                string description;
                string age;
                string causeOfDeath = wikiTextService.ResolveCauseOfDeath(entry);
                string reference = null;

                if (entry.KeepExisting)
                {
                    existingEntry = existingEntries.FirstOrDefault(ee => ee.ArticleLinkedName == entry.Id);

                    if (existingEntry == null)
                        // See WikipediaService.EvaluateExistingEntries; KeepExisting = false if null; after that collection is limited to KeepExisting = true
                        throw new ArgumentException("Existing entry not found. Should not occur per definition");

                    age = string.Empty;
                    description = existingEntry.Information.TruncLastPoint(); // including CoD, if present                    
                    reference = existingEntry.Reference;

                    if (wikiTextService.DescriptionContainsCauseOfDeath(description, causeOfDeath))
                        causeOfDeath = string.Empty;
                }
                else
                {
                    age = wikidataService.ResolveAge(entry.WikidataItem);
                    description = DetermineDescription(entry);
                    reference = referenceService.Resolve(null, deathDate, entry.WikidataItem.DateOfDeathRefs, entry.WikidataItem.Label, entry.KnownFor);
                }

                entry.ReferenceUrl = wikiTextService.GetReferenceUrlFromReferenceText(reference);
                entry.WikiText = $"*{bioLink}, {age}{description}{causeOfDeath}.{reference}";
            }
            catch (Exception e)
            {
                string message = "error: " + entry.WikidataItem.Label + " [WikipediaService.ResolveEntryText]: " + e.Message + (e.InnerException == null ? string.Empty : "\r\nInner Exception: " + e.InnerException);
                entry.WikiText = message;
                entry.NotabilityScore = 1000;
                entry.WikidataItem.MannerOfDeath = message;
                logger.LogInformation(e.Message, e);
            }
        }

        private void EnrichExistingEntries(List<ExistingEntryDto> existingEntries, IEnumerable<WikipediaListItemDto> entries, DateTime deathDate)
        {
            foreach (var existingEntry in existingEntries)
            {
                var entry = entries.FirstOrDefault(e => e.Id == existingEntry.ArticleLinkedName);

                if (entry != null)
                    EnrichFoundExistingEntry(existingEntry, entry.NotabilityScore, entry.WikidataItem.DateOfDeathRefs, null, deathDate);
                else
                {
                    // If article name does not exist in wikidata its death date in WP differs OR the name it is a redirect.                    
                    string redirectedArticleName;
                    string wikiText = wikipediaWebClient.GetWikiTextArticle(existingEntry.ArticleLinkedName, out redirectedArticleName);

                    if (redirectedArticleName == null)
                    {
                        HandleNotFoundArticleName(deathDate, existingEntry, wikiText);
                    }
                    else
                    {
                        // A page name was used in the existing entry that was redirected. Evaluate again.
                        existingEntry.ArticleLinkedName = redirectedArticleName;
                        entry = entries.FirstOrDefault(e => e.Id == existingEntry.ArticleLinkedName);

                        if (entry == null)
                            HandleNotFoundArticleName(deathDate, existingEntry, wikiText);
                        else
                        {
                            EnrichFoundExistingEntry(existingEntry, entry.NotabilityScore, entry.WikidataItem.DateOfDeathRefs, wikiText, deathDate);
                            continue;
                        }
                    }

                    // Resolve the notability score 
                    var articleMetrics = GetArticleMetrics(existingEntry.ArticleLinkedName);
                    existingEntry.NotabilityScore = articleMetrics.LinksToArticleCount * articleMetrics.SiteLinksCount;
                }

                existingEntry.ReferenceUrl = wikiTextService.GetReferenceUrlFromReferenceText(existingEntry.Reference);
            }
        }

        private void EnrichFoundExistingEntry(ExistingEntryDto existingEntry, int notabilityScore, string dateOfDeathReferences, string wikiText, DateTime deathDate)
        {
            // Try to find a reference for this existing entry so it won't be chucked if notability fails            
            bool resolveReference = false;

            // Bit iffy: has relation with ResolveReference(.. Determine flag 'resolveReference' to keep prevent downloading articles of all found existing entries.
            if (existingEntry.Reference == null)
                resolveReference = true;
            else
                if (existingEntry.Reference.Contains("sports-reference.com", StringComparison.OrdinalIgnoreCase))
                resolveReference = true;

            if (resolveReference)
            {
                existingEntry.Reference = referenceService.Resolve(existingEntry, dateOfDeathReferences, wikiText, existingEntry.ArticleLinkedName, deathDate);
            }

            existingEntry.NotabilityScore = notabilityScore;
            existingEntry.Keep = KeepExistingEntry(existingEntry, out string reason);
            existingEntry.ReasonKeepReject = reason;
        }

        private void HandleNotFoundArticleName(DateTime deathDate, ExistingEntryDto existingEntry, string wikiText)
        {
            // Cause not found: the DoD of the WP article of the existing entry differs from the corresponding wikidata item
            // Two underlying reasons:
            // 1. The entry does not belong in the evaluated Day section (which is based on deathDate and is the source of the existing entries)            
            // 2. The corresponding wikidata item states a different DoD for this article (derived conlusion; not actually chekced)

            existingEntry.Keep = false;

            if (wikiTextService.ResolveDate(wikiText, deathDate) == DateTime.MinValue)
                existingEntry.ReasonKeepReject = "Death date not found in article.";
            else
                existingEntry.ReasonKeepReject = "Wikidata states a different DoD.";
        }

        private bool KeepExistingEntry(ExistingEntryDto existingEntry, out string reason)
        {
            if (existingEntry.NotabilityScore >= minimumScore) 
            {
                reason = "Notability";
                return true;
            }
            else
            {
                if (existingEntry.Reference == null)
                {
                    reason = "Poor notability and no reference.";
                    return false;
                }
                else
                {
                    if (existingEntry.NotabilityScore * 2 < minimumScore)
                    {
                        reason = "Notability too poor in spite of reference.";
                        return false;
                    }
                    else
                    {
                        reason = "Poor notability but has reference.";
                        return true;
                    }
                }
            }
        }

        //Determine which description to use and sanitize it
        private string DetermineDescription(WikipediaListItemDto entry)
        {
            if (string.IsNullOrEmpty(entry.WikidataItem.Description) && string.IsNullOrEmpty(entry.WikipediaArticle.Description))
                return "Could not resolve description!";

            if (!string.IsNullOrEmpty(entry.WikidataItem.Description) && string.IsNullOrEmpty(entry.WikipediaArticle.Description))
                return wikiTextService.SanitizeDescription(entry.WikidataItem.Description);

            if (string.IsNullOrEmpty(entry.WikidataItem.Description) && !string.IsNullOrEmpty(entry.WikipediaArticle.Description))
                return wikiTextService.SanitizeDescription(entry.WikipediaArticle.Description);

            entry.WikipediaArticle.Description = wikiTextService.SanitizeDescription(entry.WikipediaArticle.Description);
            entry.WikidataItem.Description = wikiTextService.SanitizeDescription(entry.WikidataItem.Description);

            // Just one word?
            if (!entry.WikidataItem.Description.Contains(" "))
                return entry.WikipediaArticle.Description;

            // Does WP description contain the wikidata description?
            if (DescriptionContainsSubset(entry.WikipediaArticle.Description, entry.WikidataItem.Description))
                return entry.WikipediaArticle.Description;

            // Does wikidata description contain the WP description?
            if (DescriptionContainsSubset(entry.WikidataItem.Description, entry.WikipediaArticle.Description))
                return entry.WikidataItem.Description;

            // Favour wikidata over wp description in other cases
            return entry.WikipediaArticle.Description;
        }

        private bool DescriptionContainsSubset(string Description1, string Description2)
        {
            Description1 = Description1.Replace(" and ", ", ");
            Description2 = Description2.Replace(" and ", ", ");

            return Description1.Contains(Description2, StringComparison.OrdinalIgnoreCase);
        }

        private string GetWikiText(DateTime deathDate)
        {
            try
            {
                return wikiTextService.GetWikiTextDeathsPerMonth(deathDate, false);
            }
            catch (WikipediaPageNotFoundException)
            {
                // Deaths per month article does not exist (yet)
                return null;
            }
        }

        private IEnumerable<ExistingEntryDto> GetExistingEntries(string wikiText, DateTime deathDate, bool checkRedirect)
        {
            if (wikiText == null)
                return new List<ExistingEntryDto>();

            wikiText = wikiTextService.GetDaySectionOfMonthList(wikiText, deathDate.Day);

            IEnumerable<string> deceasedText = wikiTextService.GetDeceasedTextAsList(wikiText);
            IEnumerable<ExistingEntryDto> deceased = deceasedText.Select(e => ParseEntry(e, deathDate, checkRedirect));

            return deceased;
        }

        private void EvaluateExistingEntries(WikipediaListItemDto entry, List<ExistingEntryDto> existingEntries)
        {
            var existingEntry = existingEntries.FirstOrDefault(ee => ee.ArticleLinkedName == entry.Id);

            if (existingEntry == null)
            {
                entry.KeepExisting = false;
                return;
            }
            entry.KeepExisting = KeepExistingEntry(existingEntry, out _);
        }

        private ExistingEntryDto ParseEntry(string entryText, DateTime deathDate, bool checkRedirect)
        {
            var linkedName = GetLinkedNameFromEntryText(entryText, checkRedirect);

            return new ExistingEntryDto
            {
                DateOfDeath = deathDate,
                WikiText = entryText,
                Uri = ResolveUri(linkedName),
                ArticleName = wikiTextService.GetNameFromEntryText(entryText, false),
                ArticleLinkedName = linkedName,
                Information = wikiTextService.GetInformationFromEntryText(entryText),
                Reference = wikiTextService.GetReferencesFromEntryText(entryText),
            };
        }

        public string GetLinkedNameFromEntryText(string entryText, bool checkRedirect)
        {
            if (checkRedirect)
            {
                return wikiTextService.GetNameFromEntryText(entryText, true);
            }
            else
            { 
                string text = entryText.Substring("[[".Length, entryText.IndexOf("]]") - "]]".Length);
                int pos = text.IndexOf('|');
                return (pos < 0) ? text : text.Substring(0, pos);
            }
        }

        private string ResolveUri(string articleName)
        {
            return "https://en.wikipedia.org/wiki/" + articleName.Replace(" ", "_");
        }

        private void ResolveArticleData(WikipediaListItemDto entry)
        {
            try
            {
                // First determine the url in case of error. BTW; no relation with statement that follows
                entry.WikipediaArticle.Uri = ResolveUri(entry.Id);

                string wikiText = wikipediaWebClient.GetWikiTextArticle(entry.WikidataItem.ArticleName, out string redirectedArticle);

                if (redirectedArticle != null)
                    throw new WikipediaPageNotFoundException($"Article name '{entry.WikidataItem.ArticleName}' in Wikidata results in a redirect to '{redirectedArticle}'! Investigate.");

                entry.WikipediaArticle.DateOfBirth = wikiTextService.ResolveDateOfBirth(entry, wikiText);
                entry.WikipediaArticle.Description = wikiTextService.ResolveDescription(wikiText);
                entry.WikipediaArticle.DateOfDeath = wikiTextService.ResolveDate(wikiText, entry.WikidataItem.DateOfDeath);
                entry.WikipediaArticle.CauseOfDeath = wikiTextService.ResolveCauseOfDeath(wikiText);

                // Tro to determine why they are notable
                var knownFor = wikiTextService.ResolveKnownFor(wikiText, entry.WikipediaArticle.Description);
                if (knownFor == null)
                    knownFor = wikiTextService.ResolveKnownFor(wikiText, entry.WikidataItem.Description);
                entry.KnownFor = knownFor;
            }
            catch (Exception e)
            {
                string message = "error: " + entry.WikidataItem.Label + " [WikipediaService.ResolveArticleData]: " + e.Message + (e.InnerException == null ? string.Empty : "\r\nInner Exception: " + e.InnerException);
                entry.WikiText = message;
                entry.NotabilityScore = 1000;
                entry.WikidataItem.MannerOfDeath = message;
                logger.LogInformation(e.Message, e);
            }
        }

        private void Purge(List<WikipediaListItemDto> entries)
        {
            WikipediaListItemDto correctEntry;
            while (ContainsRemovableEntry(entries, out correctEntry))
            {
                int count = entries.Count;
                entries.RemoveAll(e => e.Id == correctEntry.Id && e.WikidataItem.DateOfBirth != e.WikipediaArticle.DateOfBirth);


                if (entries.Count == count)
                {
                    // The 'Mas Oyama' issue
                    throw new InvalidWikipediaPageException($"Exceptional situation: Multiple DoB's in WP article that match the multiple DoB's in the corresponding WikiData item! Entry: {correctEntry.WikipediaArticle.Name}. Fix it.");
                }
            }
        }

        private bool ContainsRemovableEntry(List<WikipediaListItemDto> entries, out WikipediaListItemDto correctEntry)
        {
            // An entry is removable when
            // 1. Invalid date of birth. Conditions:           
            //    - multiple entries regarding the item exist (result of SPARQ query: multiple dates of birth)
            //    - one entry contains the correct DoB (wikidata DoB == WP DoB)
            // In that case remove the other entry/entries containing the invalid date of birth.
            // In case no matching date is found in WP than don't do anything since something else is going on.
            correctEntry = null;

            foreach (var entry in entries)
            {
                var groupedData = entries.GroupBy(e => e.Id)
                            .Where(g => g.Count() > 1)
                            .Where(e => e.Key == entry.Id).ToList();

                foreach (var group in groupedData)
                {
                    bool incorrectDoBFound = false;

                    foreach (var groupedItem in group)
                    {
                        if (groupedItem.WikidataItem.DateOfBirth == groupedItem.WikipediaArticle.DateOfBirth)
                            correctEntry = groupedItem;
                        else
                            incorrectDoBFound = true;

                        if (correctEntry != null && incorrectDoBFound)
                            return true;
                    }
                }
            }

            return false;
        }

        // Initialize the entries with the id, the wikidata item, an initialized WikipediaArticle object and the 4th score.
        private List<WikipediaListItemDto> InitializeEntries(IEnumerable<WikidataItemDto> wikidataItems)
        {
            var entries = new List<WikipediaListItemDto>();

            foreach (var item in wikidataItems)
            {
                item.DateOfBirth = wikidataService.ResolveDateOfBirth(item);
                item.Label = wikidataService.ResolveItemLabel(item);
                item.Description = wikidataService.ResolveItemDescription(item);
                item.CauseOfDeath = wikidataService.ResolveItemCauseOfDeath(item);
                item.DateOfDeathRefs = wikidataService.SanitizeDateOfDeathReferences(item);

                var article = InitializeWikipediaArticle(item.ArticleName);

                entries.Add(
                    new WikipediaListItemDto
                    {
                        Id = item.ArticleName,
                        NotabilityScore = item.SiteLinksCount * article.LinksToArticleCount,
                        WikidataItem = item,
                        WikipediaArticle = article
                    });
            }

            return entries;
        }

        private WikipediaArticleDto InitializeWikipediaArticle(string articleName)
        {
            try
            {
                int linksToArticleCount = 0;

                if (articleName != null)
                    linksToArticleCount = wikipediaWebClient.GetWikimediaSearchDirectLinkCount(articleName);

                return new WikipediaArticleDto
                {
                    Name = articleName,
                    LinksToArticleCount = linksToArticleCount
                };
            }
            catch (Exception e)
            {
                throw new Exception($"WikipediaService.InitializeWikipediaArticle('{articleName}'): error calling toolforgeService.GetWikilinksInfo('{articleName}'). Message: {e.Message}", e);
            }
        }

        private int ResolveScoreNumberFour(List<WikipediaListItemDto> entries)
        {
            var uniqueEntries = entries.GroupBy(e => e.Id).Select(e => e.FirstOrDefault()).OrderByDescending(e => e.NotabilityScore).ToList();

            if (!uniqueEntries.Any())
                return 0;

            if (uniqueEntries.Count <= 4)
                return uniqueEntries.Last().NotabilityScore;

            return uniqueEntries.ElementAt(3).NotabilityScore;
        }
    }
}
