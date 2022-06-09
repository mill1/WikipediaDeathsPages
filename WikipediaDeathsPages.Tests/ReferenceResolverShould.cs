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

        [Theory(DisplayName = "Resolve Wikidata primary references")]
        [InlineData(
            "Encyclopædia Britannica Online (1)",
            "Utpal Dutt",
            "enwiki~!Encyclopædia Britannica Online~!Encyclopædia Britannica Online ID: biography/Utpal-Dutt~!subject named as: Utpal Dutt",
            "title=Utpal Dutt |url=https://www.britannica.com/biography/Utpal-Dutt |website=britannica.com"
        )]
        [InlineData(
            "Encyclopædia Britannica Online (2)",
            "Donald Davie",
            "enwiki~!reference URL: https://www.britannica.com/biography/Donald-Alfred-Davie",
            "title=Donald Davie |url=https://www.britannica.com/biography/Donald-Alfred-Davie |website=britannica.com"
        )]
        //[InlineData(
        //    "Internet Broadway Database (IBDB)",
        //    "Tom Fuccello",
        //    "enwiki~!Internet Broadway Database~!retrieved: 2017-10-09T00:00:00Z~!Internet Broadway Database person ID: 88297~!subject named as: Tom Fuccello",
        //    "title=Tom Fuccello - Broadway Cast & Staff - IBDB |url=https://www.ibdb.com/broadway-cast-staff/88297 |website=ibdb.com"
        //)]
        //[InlineData(
        //    "Spanish Biographical Dictionary (DB~e)",
        //    "Emilio Botín",
        //    "Spanish Biographical Dictionary~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Emilio Botín-Sanz de Sautuola y López~!Spanish Biographical Dictionary ID: 19291/emilio-botin-sanz-de-sautuola-y-lopez",
        //    "title=Emilio Botín - DB~e |url=https://dbe.rah.es/biografias/19291/emilio-botin-sanz-de-sautuola-y-lopez |website=dbe.rah.es |publisher=Real Academia de la Historia"
        //)]
        //[InlineData(
        //    "Biografisch Portaal",
        //    "Piet Engels",
        //    "Biografisch Portaal~!Biografisch Portaal van Nederland ID: 26363517~!reference URL: http://www.biografischportaal.nl/persoon/26363517~!title: Peter Joseph Engels",
        //    "title=Piet Engels |url=http://www.biografischportaal.nl/persoon/26363517 |website=biografischportaal.nl"
        //)]
        //[InlineData(
        //    "FemBio",
        //    "Catherine Collard",
        //    "enwiki~!FemBio~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Cathérine Collard~!FemBio ID: 6165",
        //    "title=Frauendatenbank fembio.org |url=https://www.fembio.org/biographie.php/frau/frauendatenbank?fem_id=6165 |website=fembio.org"
        //)]
        //[InlineData(
        //    "Filmportal",
        //    "Gerry Sundquist",
        //    "enwiki~!filmportal.de~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Gerry Sundquist~!Filmportal ID: 8ec7d1f8c8774fd8bcac2d74b60413c2",
        //    "title=Gerry Sundquist - filmportal.de |url=https://www.filmportal.de/person/8ec7d1f8c8774fd8bcac2d74b60413c2 |website=filmportal.de"
        //)]
        //[InlineData(
        //    "Fichier des décès",
        //    "Philippe Pradayrol",
        //    "enwiki~!Fichier des personnes décédées~!retrieved: 2021-07-23T00:00:00Z~!reference URL: https://deces.matchid.io/id/Jwjy9PxtUQEm",
        //    "title=matchID - Philippe Pradayrol |url=https://deces.matchid.io/id/Jwjy9PxtUQEm |website=[[Fichier des personnes décédées|Fichier des décès]]"
        //)]
        public void ResolveWikidataPrimaryReferences(string source, string articleLabel, string dateOfDeathRefs, string expectedSubstring)
        {
            Debug.WriteLine($"##### Testing source {source}...");
            var actualString = referenceResolver.Resolve(DateTime.MinValue, dateOfDeathRefs, articleLabel, null);
            Assert.Contains(expectedSubstring, actualString);
        }

        // Websites IBDB, FemBio and Fichier des décès return status code 200 (+ redirect) instead of HttpStatusCode.NotFound (404) in case of a request to a non-existent url.
        // So there is no need to check the validity of the url's regarding these websites
        [Theory(DisplayName = "Handle Wikidata 404 references")]
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
        public void WikidataPrimaryReferenceNotFound(string source, string dateOfDeathRefs)
        {
            Debug.WriteLine($"##### Testing source {source}...");
            var actualString = referenceResolver.Resolve(DateTime.MinValue, dateOfDeathRefs, "John Doe", null);
            Assert.Null(actualString);
        }




        [Theory(DisplayName = "Resolve Wikidata secondary references")]
        [InlineData(
            "Library of Congress",
            "Muhlis Akarsu",
            "enwiki~!Library of Congress authority ID: no2017084878~!Library of Congress Authorities~!retrieved: 2020-03-05T00:00:00Z",
            "title=Muhlis Akarsu - Library of Congress |url=https://id.loc.gov/authorities/names/no2017084878 |website=id.loc.gov"
        )]
        public void ResolveWikidataSecondaryReferences(string source, string articleLabel, string dateOfDeathRefs, string expectedSubstring)
        {
            Debug.WriteLine($"##### Testing source {source}...");
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
