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
       
        [Fact(DisplayName = "Resolve The Independent reference")]
        public void ResolveTheIndependentReference()
        {
            const string name = "Stanley Woods";
            const string url = "https://www.independent.co.uk/news/people/obituary-stanley-woods-1488284.html";
            var deathDate = new DateTime(1993, 7, 28);
            var dateOfDeathRef = $"enwiki~!reference URL: {url}";

            var expectedSubstring1 = $"author1=Jim Reynolds |author-link1= |title=Obituary: Stanley Woods |url={url}";
            var expectedSubstring2 = $"work=[[The Independent]] |language= |date=30 July 1993";
            var actualString = referenceResolver.Resolve(deathDate, dateOfDeathRef, name, null);

            Assert.Contains(expectedSubstring1, actualString);
            Assert.Contains(expectedSubstring2, actualString);
        }

        [Theory(DisplayName = "Resolve Wikidata references")]
        [InlineData(
            "Encyclopædia Britannica Online (1)",
            "Utpal Dutt",
            "enwiki~!Encyclopædia Britannica Online~!Encyclopædia Britannica Online ID: biography/Utpal-Dutt~!subject named as: Utpal Dutt",
            "title=Utpal Dutt |url=https://www.britannica.com/biography/Utpal-Dutt |website=britannica.com"
        )]
        [InlineData(
            "Encyclopædia Britannica Online (2)",
            "Donald Davie",
            "enwiki~!reference URL: http://www.britannica.com/biography/Donald-Alfred-Davie",
            "title=Donald Davie |url=http://www.britannica.com/biography/Donald-Alfred-Davie |website=britannica.com"
        )]
        [InlineData(
            "Encyclopædia Britannica Online (3)",
            "Donald Davie",
            "enwiki~!reference URL: https://www.britannica.com/biography/Donald-Alfred-Davie",
            "title=Donald Davie |url=https://www.britannica.com/biography/Donald-Alfred-Davie |website=britannica.com"
        )]
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
        [InlineData(
            "FemBio",
            "Catherine Collard",
            "enwiki~!FemBio~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Cathérine Collard~!FemBio ID: 6165",
            "title=Frauendatenbank fembio.org |url=https://www.fembio.org/biographie.php/frau/frauendatenbank?fem_id=6165 |website=fembio.org"
        )]
        [InlineData(
            "Filmportal",
            "Gerry Sundquist",
            "enwiki~!filmportal.de~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Gerry Sundquist~!Filmportal ID: 8ec7d1f8c8774fd8bcac2d74b60413c2",
            "title=Gerry Sundquist - filmportal.de |url=https://www.filmportal.de/person/8ec7d1f8c8774fd8bcac2d74b60413c2 |website=filmportal.de"
        )]
        [InlineData(
            "Fichier des décès",
            "Philippe Pradayrol",
            "enwiki~!Fichier des personnes décédées~!retrieved: 2021-07-23T00:00:00Z~!reference URL: https://deces.matchid.io/id/Jwjy9PxtUQEm",
            "title=matchID - Philippe Pradayrol |url=https://deces.matchid.io/id/Jwjy9PxtUQEm |website=[[Fichier des personnes décédées|Fichier des décès]]"
        )]
        [InlineData(
            "Library of Congress (LoC)",
            "John Brooks",
            "enwiki~!Library of Congress authority ID: n79063009~!Library of Congress Authorities~!retrieved: 2019-12-16T00:00:00Z",
            "title=John Brooks - Library of Congress |url=https://id.loc.gov/authorities/names/n79063009 |website=id.loc.gov"
        )]
        [InlineData(
            "Social Networks and Archival Context (snac)",
            "Eric Irvin",
            "enwiki~!SNAC~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Eric Irvin~!SNAC ARK ID: w62z2kg5",
            "title=Eric Irvin - Social Networks and Archival Context |url=https://snaccooperative.org/ark:/99166/w62z2kg5 |website=snaccooperative.org"
        )]
        [InlineData(
            "Bibliothèque nationale de France (BnF) (1)",
            "Irving J. Moore",
            "enwiki~!BnF authorities~!retrieved: 2015-10-10T00:00:00Z~!reference URL: http://data.bnf.fr/ark:/12148/cb14122678c",
            "title=Irving J. Moore |url=http://data.bnf.fr/ark:/12148/cb14122678c |website=data.bnf.fr |publisher=Bibliothèque nationale de France"
        )]
        [InlineData(
            "Bibliothèque nationale de France (BnF) (2)",
            "Red Prysock",
            "enwiki~!BnF authorities~!retrieved: 2015-10-10T00:00:00Z~!reference URL: https://data.bnf.fr/ark:/12148/cb13968003b",
            "title=Red Prysock |url=https://data.bnf.fr/ark:/12148/cb13968003b |website=data.bnf.fr |publisher=Bibliothèque nationale de France"
        )]
        [InlineData(
            "Bibliothèque nationale de France (BnF) (3)",
            "Isabela Corona",
            "BnF authorities~!Bibliothèque nationale de France ID: 15531676g~!subject named as: Isabela Corona",
            "title=Isabela Corona |url=https://catalogue.bnf.fr/ark:/12148/cb15531676g |website=catalogue.bnf.fr |publisher=Bibliothèque nationale de France"
        )]
        public void ResolveWikidataReferences(string source, string articleLabel, string dateOfDeathRefs, string expectedSubstring)
        {
            Debug.WriteLine($"##### Testing source {source}...");
            var actualString = referenceResolver.Resolve(DateTime.MinValue, dateOfDeathRefs, articleLabel, null);
            Assert.Contains(expectedSubstring, actualString);
        }

        // Websites IBDB, FemBio, Fichier des décès and SNAC all return status code 200 (+ redirect) instead of HttpStatusCode.NotFound (404) in case of a request to a non-existent url.
        // F.i. instead of a 404 status code SNAC returns a 200 code: 'Constellation with ark http://n2t.net/ark:/99166/NOTFOUND does not have a published version'.
        // Also, GET requests to site of Bibliothèque nationale de France (BnF) always result in a 303 (redirect) Don't know why.
        // Ergo: there is no need to check the validity of url's regarding these websites.
        [Theory(DisplayName = "Handle Wikidata 404 references")]
        [InlineData(
            "Encyclopædia Britannica Online (1)",
            "enwiki~!Encyclopædia Britannica Online~!Encyclopædia Britannica Online ID: biography/NOTFOUND~!subject named as: Utpal Dutt"
        )]
        [InlineData(
            "Encyclopædia Britannica Online (2)",
            "enwiki~!reference URL: http://www.britannica.com/biography/NOTFOUND"
        )]
        [InlineData(
            "Encyclopædia Britannica Online (3)",
            "enwiki~!reference URL: https://www.britannica.com/biography/NOTFOUND"
        )]
        [InlineData(
            "The Independent",
            "enwiki~!reference URL: https://www.independent.co.uk/news/people/obituary-NOTFOUND.html"
        )]
        [InlineData(
            "Spanish Biographical Dictionary (DB~e)",
            "Spanish Biographical Dictionary~!subject named as: Emilio Botín-Sanz de Sautuola y López~!Spanish Biographical Dictionary ID: NOTFOUND"
        )]
        [InlineData(
            "Biografisch Portaal",
            "Biografisch Portaal~!Biografisch Portaal van Nederland ID: NOTFOUND~!reference URL: http://www.biografischportaal.nl/persoon/NOTFOUND~!title: Peter Joseph Engels"
        )]
        [InlineData(
            "Filmportal",
            "enwiki~!filmportal.de~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Gerry Sundquist~!Filmportal ID: NOTFOUND"
        )]
        [InlineData(
            "Library of Congress (LoC)",
            "enwiki~!Library of Congress authority ID: NOTFOUND~!Library of Congress Authorities~!retrieved: 2019-12-16T00:00:00Z"
        )]
        public void WikidataPrimaryReferenceNotFound(string source, string dateOfDeathRefs)
        {
            Debug.WriteLine($"##### Testing source {source}...");
            var actualString = referenceResolver.Resolve(DateTime.MinValue, dateOfDeathRefs, "John Doe", null);
            Assert.Null(actualString);
        }

        // TODO (sports sites)
        // Check multiple occurrences name

        [Fact(DisplayName = "Resolve Baseball reference")]
        public void ResolveBaseballReference()
        {
            const string name = "Tsunemi Tsuda";
            var deathDate = new DateTime(1993, 7, 20);            

            var expectedSubstring = "title=Tsunemi Tsuda Stats - Baseball-Reference.com |url=https://www.baseball-reference.com/search/search.fcgi?search=Tsunemi+Tsuda |website=baseball-reference.com";
            var actualString = referenceResolver.Resolve(deathDate, null, name, "Baseball");

            Assert.Contains(expectedSubstring, actualString);
        }


        [Fact(DisplayName = "Resolve Olympic reference")]
        public void ResolveOlympicReference()
        {
            const string name = "Fanny Blankers-Koen";
            var deathDate = new DateTime(2004, 1, 25);      
            wikipediaReferencesMock.Setup(_ => _.GetIdsOfName(name)).Returns(new List<int> { 73711 });

            var expectedSubstring = $"title=Olympedia – {name} |url=https://www.olympedia.org/athletes/73711 |website=olympedia.org";
            var actualString = referenceResolver.Resolve(deathDate, null, name, "Olympics");

            Assert.Contains(expectedSubstring, actualString);
        }

    }
}
