using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WikipediaDeathsPages.Data.Interfaces;
using WikipediaDeathsPages.Service;
using Xunit;

namespace WikipediaDeathsPages.Tests
{
    public class ReferenceServiceShould
    {
        private readonly ReferenceService referenceService;
        private readonly Mock<IWikipediaReferences> wikipediaReferencesMock;

        public ReferenceServiceShould()
        {
            wikipediaReferencesMock = new Mock<IWikipediaReferences>();
            referenceService = new ReferenceService(wikipediaReferencesMock.Object, null, null);
        }

        // reference URL: http://www.theguardian.com/news/2001/apr/18/guardianobituaries.books
        [Fact(DisplayName = "Resolve The Guardian reference")]
        public void ResolveTheGuardianReference()
        {
            const string name = "Beryl Gilroy";
            const string url = "https://www.theguardian.com/news/2001/apr/18/guardianobituaries.books";
            var deathDate = new DateTime(2001, 4, 4);
            var dateOfDeathRef = $"enwiki~!reference URL: {url}";

            var expectedSubstring1 = $"author1=Peter D Fraser |author-link1= |title={name} |url={url}";
            var expectedSubstring2 = $"work=[[The Guardian]] |language= |date=18 April 2001";
            var actualString = referenceService.Resolve(deathDate, dateOfDeathRef, name, null);

            Assert.Contains(expectedSubstring1, actualString);
            Assert.Contains(expectedSubstring2, actualString);
        }

        // The Independent is the only non-sports website whose response is checked for the expected date death.
        [Fact(DisplayName = "Resolve The Independent reference (1)")]
        public void ResolveTheIndependentReferencePeople()
        {
            const string name = "Stanley Woods";
            const string url = "https://www.independent.co.uk/news/people/obituary-stanley-woods-1488284.html";
            var deathDate = new DateTime(1993, 7, 28);
            var dateOfDeathRef = $"enwiki~!reference URL: {url}";

            var expectedSubstring1 = $"author1=Jim Reynolds |author-link1= |title=Obituary: {name} |url={url}";
            var expectedSubstring2 = $"work=[[The Independent]] |language= |date=30 July 1993";
            var actualString = referenceService.Resolve(deathDate, dateOfDeathRef, name, null);

            Assert.Contains(expectedSubstring1, actualString);
            Assert.Contains(expectedSubstring2, actualString);
        }

        [Fact(DisplayName = "Resolve The Independent reference (2)")]
        public void ResolveTheIndependentReferenceIncoming()
        {
            const string name = "Harold Shepherdson";
            const string url = "https://www.independent.co.uk/incoming/obituary-harold-shepherdson-5649167.html";
            var deathDate = new DateTime(1995, 9, 13);
            var dateOfDeathRef = $"enwiki~!reference URL: {url}";

            var expectedSubstring1 = $"author1=Ivan Ponting |author-link1= |title=Obituary: Harold Shepherdson |url={url}";
            var expectedSubstring2 = $"work=[[The Independent]] |language= |date=14 September 1995";
            var actualString = referenceService.Resolve(deathDate, dateOfDeathRef, name, null);

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
            "enwiki~!reference URL: http://www.britannica.com/biography/Donald-Alfred-Davie",  // http in Wikidata
            "title=Donald Davie |url=http://www.britannica.com/biography/Donald-Alfred-Davie |website=britannica.com"
        )]
        [InlineData(
            "Encyclopædia Britannica Online (3)",
            "Donald Davie",
            "enwiki~!reference URL: https://www.britannica.com/biography/Donald-Alfred-Davie", // https
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
            "Fichier des décès (1)",
            "Armand Vaquerin",
            "enwiki~!Fichier des personnes décédées ID (matchID): CV5_zvnsSD0U",
            "title=matchID - Armand Vaquerin |url=https://deces.matchid.io/id/CV5_zvnsSD0U |website=[[Fichier des personnes décédées|Fichier des décès]]"
        )]
        [InlineData(
            "Fichier des décès (2)",
            "Philippe Pradayrol",
            "enwiki~!Fichier des personnes décédées~!retrieved: 2021-07-23T00:00:00Z~!reference URL: http://deces.matchid.io/id/Jwjy9PxtUQEm",
            "title=matchID - Philippe Pradayrol |url=http://deces.matchid.io/id/Jwjy9PxtUQEm |website=[[Fichier des personnes décédées|Fichier des décès]]"
        )]
        [InlineData(
            "Fichier des décès (3)",
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
            "enwiki~!BnF authorities~!retrieved: 2015-10-10T00:00:00Z~!reference URL: http://data.bnf.fr/ark:/12148/cb14122678c",  // http
            "title=Irving J. Moore |url=http://data.bnf.fr/ark:/12148/cb14122678c |website=data.bnf.fr |publisher=Bibliothèque nationale de France"
        )]
        [InlineData(
            "Bibliothèque nationale de France (BnF) (2)",
            "Red Prysock",
            "enwiki~!BnF authorities~!retrieved: 2015-10-10T00:00:00Z~!reference URL: https://data.bnf.fr/ark:/12148/cb13968003b", // https
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
            var actualString = referenceService.Resolve(DateTime.MinValue, dateOfDeathRefs, articleLabel, null);
            Assert.Contains(expectedSubstring, actualString);
        }

        // Make sure that all requests  non-existent Wikidata-url's result in a null value of the returned reference.
        // Nb: Websites IBDB, FemBio, Fichier des décès and SNAC all return status code 200 (+ redirect) instead of 404 in case of a request to a non-existent url.
        // F.i. instead of a 404 status code SNAC returns a 200 code: 'Constellation with ark http://n2t.net/ark:/99166/NOTFOUND does not have a published version'.
        // Also, GET requests to site of Bibliothèque nationale de France (BnF) always result in a 303 (redirect) Don't know why.
        // Ergo: there is no need to check the validity of url's regarding these websites since we don't inspect them for death date anyway in case of a 200 response.
        [Theory(DisplayName = "Return null in case of non-existent url (in Wikidata)")]
        [InlineData("Encyclopædia Britannica (1)", "enwiki~!Encyclopædia Britannica Online~!Encyclopædia Britannica Online ID: biography/NOTFOUND")]
        [InlineData("Encyclopædia Britannica (2)", "enwiki~!reference URL: http://www.britannica.com/biography/NOTFOUND")]
        [InlineData("Encyclopædia Britannica (3)", "enwiki~!reference URL: https://www.britannica.com/biography/NOTFOUND")]
        [InlineData("The Independent", "enwiki~!reference URL: https://www.independent.co.uk/news/people/obituary-NOTFOUND.html")]
        [InlineData("Spanish Biographical Dictionary (DB~e)", "Spanish Biographical Dictionary~!subject named as: Not Found~!Spanish Biographical Dictionary ID: NOTFOUND")]
        [InlineData("Biografisch Portaal", "Biografisch Portaal~!Biografisch Portaal van Nederland ID: NOTFOUND~!reference URL: http://www.biografischportaal.nl/persoon/NOTFOUND~!title: Not Found")]
        [InlineData("Filmportal", "enwiki~!filmportal.de~!retrieved: 2017-10-09T00:00:00Z~!subject named as: Not Found~!Filmportal ID: NOTFOUND")]        
        public void ReturnNullIfWikidataUrlNotFound(string source, string dateOfDeathRefs)
        {
            Debug.WriteLine($"##### Testing source {source}...");
            var actualString = referenceService.Resolve(DateTime.MinValue, dateOfDeathRefs, "John Doe", null);
            Assert.Null(actualString);
        }

        [Fact(DisplayName = "Resolve Olympic reference")]
        public void ResolveOlympicReference()
        {
            const string name = "Fanny Blankers-Koen";
            var deathDate = new DateTime(2004, 1, 25);
            wikipediaReferencesMock.Setup(_ => _.GetIdsOfName(name)).Returns(new List<int> { 73711 });

            var expectedSubstring = $"title=Olympedia – {name} |url=https://www.olympedia.org/athletes/73711 |website=olympedia.org";
            var actualString = referenceService.Resolve(deathDate, null, name, "Olympics");

            Assert.Contains(expectedSubstring, actualString);
        }

        [Theory(DisplayName = "Resolve website references")]
        [InlineData(
            "Baseball",
            "Baseball-Reference.com (single occurrence of name)",
            "Tsunemi Tsuda",
            "1993-7-20",
            "title=Tsunemi Tsuda Stats - Baseball-Reference.com |url=https://www.baseball-reference.com/search/search.fcgi?search=Tsunemi+Tsuda |website=baseball-reference.com"
        )]
        [InlineData(
            "Baseball",
            "Baseball-Reference.com (multiple occurrences)",
            "George Smith",
            "1987-6-15",
            "title=George Smith Stats - Baseball-Reference.com |url=https://www.baseball-reference.com/players/s/smithge04.shtml |website=baseball-reference.com"
        )]
        [InlineData(
            "American football",
            "Pro-Football-Reference.com (single occurrence of name)",
            "Abe Shires",
            "1993-7-23",
            "title=Abe Shires Stats - Pro-Football-Reference.com |url=https://www.pro-football-reference.com/search/search.fcgi?search=Abe+Shires |website=pro-football-reference.com"
        )]
        [InlineData(
            "American football",
            "Pro-Football-Reference.com (multiple occurrences)",
            "George Smith",
            "1986-3-5",
            "title=George Smith Stats - Pro-Football-Reference.com |url=https://www.pro-football-reference.com/players/S/SmitGe22.htm |website=pro-football-reference.com"
        )]
        [InlineData(
            "Basketball",
            "Basketball-Reference.com (single occurrence of name)",
            "Reggie Lewis",
            "1993-7-27",
            "title=Reggie Lewis Stats - Basketball-Reference.com |url=https://www.basketball-reference.com/search/search.fcgi?search=Reggie+Lewis |website=basketball-reference.com"    // At times the redirect works. At times not.
        )]
        [InlineData(
            "Basketball",
            "Basketball-Reference.com (multiple occurrences)", // list: https://www.basketball-reference.com/search/search.fcgi?search=Eddie+Johnson
            "Eddie Johnson",
            "2020-10-26",
            "title=Eddie Johnson Stats - Basketball-Reference.com |url=https://www.basketball-reference.com/players/j/johnsed02.html |website=basketball-reference.com"
        )]
        [InlineData(
            "Hockey",
            "Hockey-Reference.com (single occurrence of name)",
            "Archie Wilcox",
            "1993-8-27",
            "title=Archie Wilcox Stats - Hockey-Reference.com |url=https://www.hockey-reference.com/search/search.fcgi?search=Archie+Wilcox |website=hockey-reference.com"
        )]
        [InlineData(
            "Hockey",
            "Hockey-Reference.com (multiple occurrences)",
            "Brian Smith ",
            "1995-8-2",
            "title=Brian Smith  Stats - Hockey-Reference.com |url=https://www.hockey-reference.com/players/s/smithbr02.html |website=hockey-reference.com"
        )]
        [InlineData(
            "Association football",
            "worldfootball.net",
            "Tony Barton",
            "1993-8-20",
            "title=Tony Barton |url=https://www.worldfootball.net/player_summary/tony-barton/ |website=worldfootball.net"
        )]
        [InlineData(
            "Cyclist",
            "procyclingstats.com",
            "Alfred Haemerlinck",
            "1993-7-10",
            "title=Alfred Haemerlinck |url=https://www.procyclingstats.com/rider/alfred-haemerlinck |website=procyclingstats.com"
        )]
        [InlineData(
            "Golfer",
            "where2golf.com",
            "Payne Stewart",
            "1999-10-25",
            "title=Payne Stewart |url=https://www.where2golf.com/whos-who/payne-stewart |website=where2golf.com"
        )]
        public void ResolveWebsiteReferences(string knownFor, string websiteName, string articleLabel, string deathDateString, string expectedSubstring)
        {
            Debug.WriteLine($"##### Testing website {websiteName}...");
            var actualString = referenceService.Resolve(DateTime.Parse(deathDateString), null, articleLabel, knownFor);
            Assert.Contains(expectedSubstring, actualString);
        }

        // Make sure that all requests to non-existent (sport) website url's result in a null value of the returned reference.
        // Even if the website returns status 200 (Ok) in case of a non-existent url we're good since (unlike most Wikdata ref sources) 
        // we check the response of (sport) websites for the date of death which will result in a null reference since it will not be found.
        [Theory(DisplayName = "Return null in case of non-existent url (to sports website)")]
        [InlineData("Baseball", "Baseball-Reference.com")]
        [InlineData("American football", "Pro-Football-Reference.com")]
        [InlineData("Basketball", "Basketball-Reference.com")]
        [InlineData("Hockey", "Hockey-Reference.com")]
        [InlineData("Olympics", "olympedia.com")]
        [InlineData("Association football", "worldfootball.net")]
        [InlineData("Cyclist", "procyclingstats.com")]
        [InlineData("Golfer", "where2golf.com")]
        public void ReturnNullIfWebsiteUrlNotFound(string knownFor, string websiteName)
        {
            Debug.WriteLine($"##### Testing website {websiteName}...");
            var actualString = referenceService.Resolve(DateTime.MinValue, null, "Noty Foundy", knownFor);
            Assert.Null(actualString);
        }
    }
}
