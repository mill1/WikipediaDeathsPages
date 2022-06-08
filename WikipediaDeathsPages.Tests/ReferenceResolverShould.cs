using Xunit;
using WikipediaDeathsPages.Service;
using Moq;
using System;
using WikipediaDeathsPages.Data.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;

namespace WikipediaDeathsPages.Tests
{
    public class ReferenceResolverShould
    {
        private readonly ReferenceResolver referenceResolver;
        private readonly Mock<IWikipediaReferences> wikipediaReferencesMock;

        public ReferenceResolverShould()
        {
            var logger = new NullLogger<ReferenceResolver>();

            wikipediaReferencesMock = new Mock<IWikipediaReferences>();
            referenceResolver = new ReferenceResolver(wikipediaReferencesMock.Object, logger);
        }

        // Do not use Theory; Fact per makes identification of broken source easier.        
        [Fact(DisplayName = "Resolve Britannica reference")]
        public void ResolveBritannicaReference()
        {
            const string name = "Utpal Dutt";
            var deathDate = new DateTime(1993, 8, 19);
            var dateOfDeathRef = "enwiki~!Encyclopædia Britannica Online~!Encyclopædia Britannica Online ID: biography/Utpal-Dutt~!subject named as: Utpal Dutt";

            var expectedSubstring = $"title={name} |url=https://www.britannica.com/biography/Utpal-Dutt |website=britannica.com";
            var actualString = referenceResolver.Resolve(deathDate, dateOfDeathRef, name, null);

            Assert.Contains(expectedSubstring, actualString);
        }

        [Fact(DisplayName = "Resolve The Independent reference")]
        public void ResolveTheIndependentReference()
        {
            const string name = "Stanley Woods";
            const string url = "https://www.independent.co.uk/news/people/obituary-stanley-woods-1488284.html";
            var deathDate = new DateTime(1993, 7, 28);
            var dateOfDeathRef = $"enwiki~!reference URL: {url}";
            
            var expectedSubstring = $"author1=Jim Reynolds |author-link1= |title=Obituary: Stanley Woods |url={url} |url-access= |access-date=8 June 2022 |work=[[The Independent]] |language= |date=30 July 1993";
            var actualString = referenceResolver.Resolve(deathDate, dateOfDeathRef, name, null);

            Assert.Contains(expectedSubstring, actualString);
        }

        [Fact(DisplayName = "Resolve Washington Post reference")]
        public void ResolveWashingtonPostReference()
        {
            const string name = "Jon Pattis";
            const string url = "https://www.washingtonpost.com/archive/local/1996/01/15/jon-pattis-58-dies/06d38941-a1f0-4c39-ab05-35ee68e362e2/";            
            var dateOfDeathRef = $"enwiki~!reference URL: {url}";

            var expectedSubstring = $"title=Obituary {name} |url={url}";
            var actualString = referenceResolver.Resolve(DateTime.MinValue, dateOfDeathRef, name, null);

            Assert.Contains(expectedSubstring, actualString);
        }

        [Theory(DisplayName = "Resolve Wikidata references")]
        [InlineData(
            "Internet Broadway Database (IBDB)",
            "Tom Fuccello",
            "enwiki~!Internet Broadway Database~!retrieved: 2017-10-09T00:00:00Z~!Internet Broadway Database person ID: 88297~!subject named as: Tom Fuccello",
            "title=Tom Fuccello - Broadway Cast & Staff - IBDB |url=https://www.ibdb.com/broadway-cast-staff/88297 |website=ibdb.com"
        )]
        [InlineData(
            "Spanish Biographical Dictionary (DB~e)",
            "Emilio Botín",
            "Spanish Biographical Dictionary~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Emilio Botín-Sanz de Sautuola y López~!Spanish Biographical Dictionary ID: 19291/emilio-botin-sanz-de-sautuola-y-lopez",
            "title=Emilio Botín - DB~e |url=https://dbe.rah.es/biografias/19291/emilio-botin-sanz-de-sautuola-y-lopez |website=dbe.rah.es |publisher=Real Academia de la Historia"
        )]
        [InlineData(
            "Biografisch Portaal",
            "Piet Engels",
            "Biografisch Portaal~!Biografisch Portaal van Nederland ID: 26363517~!reference URL: http://www.biografischportaal.nl/persoon/26363517~!title: Peter Joseph Engels",
            "title=Piet Engels |url=http://www.biografischportaal.nl/persoon/26363517 |website=biografischportaal.nl"
        )]
        public void ResolveWikidataReferences(string source, string articleLabel, string dateOfDeathRefs, string expectedSubstring)
        {
            Debug.Write($"Testing source {source}...");
            var actualString = referenceResolver.Resolve(DateTime.MinValue, dateOfDeathRefs, articleLabel, null);
            Assert.Contains(expectedSubstring, actualString);
        }

        [Fact(DisplayName = "Resolve Olympic reference")]
        public void ResolveOlympicReference()
        {
            const string name = "Fanny Blankers-Koen";
            var deathDate = new DateTime(2004, 1, 25);      
            wikipediaReferencesMock.Setup(_ => _.GetIdsOfName(name)).Returns(new List<int> { 73711 });

            var expectedSubstring = $"title=Olympedia – {name} |url=http://www.olympedia.org/athletes/73711 |website=olympedia.org";
            var actualString = referenceResolver.Resolve(deathDate, null, name, "Olympics");

            Assert.Contains(expectedSubstring, actualString);
        }

    }
}
