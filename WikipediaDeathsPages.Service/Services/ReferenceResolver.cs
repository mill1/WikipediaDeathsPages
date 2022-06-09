using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using WikipediaDeathsPages.Data.Interfaces;
using Wikimedia.Utilities.ExtensionMethods;
using WikipediaDeathsPages.Service.Helpers;
using WikipediaDeathsPages.Service.Interfaces;
using HtmlAgilityPack;
using Newtonsoft.Json;
using WikipediaDeathsPages.Service.Models;
using System.Net.Http;

/*
Loop eerst door de 'referenties'
'Volgorde': zie volgorde in methode Resolve()

(Oxford DNB: subscription needed: geen DoD op gratis excerpt https://www.oxforddnb.com/view/10.1093/ref:odnb/9780198614128.001.0001/odnb-9780198614128-e-63193)
(Dictionary of Irish Biography: too complicated/error prone for new: https://www.dib.ie/biography/mitchel-charles-gerald-a5832)

Indien geen match: check entry.KnownFor
Eén waarde mbt KnowFor: volgorde wordt dus bepaald in WikipediaService.ResolveKnownFor():

Tot slot NYT tool er overheen halen.
 */

namespace WikipediaDeathsPages.Service
{
    public class ReferenceResolver : IReferenceResolver
    {
        private readonly IWikipediaReferences wikipediaReferences;
        private readonly ILogger<ReferenceResolver> logger;
        private readonly WebClient webClient;
        private readonly CultureInfo cultureInfo;

        public ReferenceResolver(IWikipediaReferences wikipediaReferences, ILogger<ReferenceResolver> logger)
        {
            this.wikipediaReferences = wikipediaReferences;
            this.logger = logger;
            cultureInfo = new CultureInfo("en-US");

            webClient = new WebClient();
            webClient.Headers.Clear();
            webClient.Headers.Add("User-Agent", "C# application");
        }

        public string Resolve(DateTime deathDate, string dateOfDeathreferences, string articleLabel, string knownFor)
        {
            var referenceItems = GetReferenceItems(dateOfDeathreferences);

            // 1. Try get references from preferred sources stated in Wikidata
            var reference = GetReferenceFromWikidatRefItemsFirst(articleLabel, referenceItems, deathDate);

            if (reference == null)
                // 2. Try get references from (sports) websites
                reference = GetReferenceFromWebsites(articleLabel, knownFor, deathDate);

            if (reference == null)
                // 3. Try get references from secondary sources stated in Wikidata
                reference = GetReferenceFromWikidatRefItemsSecond(articleLabel, referenceItems);

            return reference;
        }

        private string GetReferenceFromWebsites(string articleLabel, string knownFor, DateTime deathDate)
        {
            switch (knownFor)
            {
                case "Baseball":
                    return GetBaseballReference(articleLabel, deathDate);
                case "American football":
                    return GetProFootballReference(articleLabel, deathDate);
                case "Basketball":
                    return GetBasketballReference(articleLabel, deathDate);
                case "Hockey":
                    return GetHockeyReference(articleLabel, deathDate);
                case "Olympics":
                    return GetOlympediaReference(articleLabel, deathDate);
                case "Association football":
                    return GetAssociationFootballReference(articleLabel, deathDate);
                case "Cyclist":
                    return GetCyclistReference(articleLabel, deathDate);
                case "Golfer":
                    return GetGolferReference(articleLabel, deathDate);
                default:
                    break;
            }
            return null;
        }

        private string GetBaseballReference(string articleLabel, DateTime deathDate)
        {
            return GetSportsReference(articleLabel, "baseball-reference.com", deathDate);
        }

        private string GetProFootballReference(string articleLabel, DateTime deathDate)
        {
            return GetSportsReference(articleLabel, "pro-football-reference.com", deathDate);
        }

        private string GetHockeyReference(string articleLabel, DateTime deathDate)
        {
            return GetSportsReference(articleLabel, "hockey-reference.com", deathDate);
        }

        private string GetBasketballReference(string articleLabel, DateTime deathDate)
        {
            return GetSportsReference(articleLabel, "basketball-reference.com", deathDate);
        }

        private string GetOlympediaReference(string articleLabel, DateTime deathDate)
        {
            List<string> names = GetNameAlternatives(articleLabel);

            foreach (var name in names)
            {
                var OlympediaIds = wikipediaReferences.GetIdsOfName(name);

                if (OlympediaIds != null)
                {
                    foreach (var id in OlympediaIds)
                    {
                        // e.g.    https://www.olympedia.org/athletes/73711
                        var url = "https://www.olympedia.org/athletes/" + id;
                        var response = webClient.DownloadString(url);                        

                        if (response.Contains(GetLongEnDateText(deathDate)))
                            return GenerateWebReference($"Olympedia – {articleLabel}", url, "olympedia.org", DateTime.Today, DateTime.MinValue, publisher: "[[OlyMADMen]]");
                    }
                }
            }
            return null;
        }

        private List<string> GetNameAlternatives(string name)
        {
            List<string> names = new List<string> { name };

            int pos = name.IndexOf(" ");

            if (pos == -1)
                return names;

            var givenName = name.Substring(0, pos);
            var surName = name.Substring(pos + 1);

            // Check diminutive of name, if any.
            var diminutives = Diminutives.GetNames(givenName);

            if (diminutives.Count == 0)
                return names;

            foreach (var diminutive in diminutives)
                names.Add(diminutive + " " + surName);

            return names;
        }

        private string GetCyclistReference(string articleLabel, DateTime deathDate)
        {
            // e.g.    https://www.procyclingstats.com/rider/jacques-anquetil
            var url = "https://www.procyclingstats.com/rider/" + articleLabel.ToLower().Replace(" ", "-");

            var response = webClient.DownloadString(url);

            if (response.Contains($"passed away {deathDate.ToString("yyyy-MM-dd")}"))
                return GenerateWebReference(articleLabel, url, "procyclingstats.com", DateTime.Today, DateTime.MinValue);

            return null;
        }

        private string GetGolferReference(string articleLabel, DateTime deathDate)
        {
            // e.g.    https://www.where2golf.com/whos-who/payne-stewart/
            var url = "https://www.where2golf.com/whos-who/" + articleLabel.ToLower().Replace(" ", "-");

            var response = DownloadString(url, true);

            if (response == null)
                return null;

            var doD = deathDate;
            var search = $"Died on {GetWhere2GolfMonthName(doD)} {doD.Day}, {doD.Year}";

            if (response.Contains(search))
                return GenerateWebReference(articleLabel, url, "where2golf.com", DateTime.Today, DateTime.MinValue);

            return null;
        }        

        private string GetAssociationFootballReference(string articleLabel, DateTime deathDate)
        {
            // e.g.    https://www.worldfootball.net/player_summary/bob-paisley/
            var url = "https://www.worldfootball.net/player_summary/" + articleLabel.ToLower().Replace(" ", "-") + "/";

            try
            {
                var response = webClient.DownloadString(url);

                if (response.Contains(@$"&dagger; {deathDate.ToString("dd.MM.yyyy")}"))
                    return GenerateWebReference(articleLabel, url, "worldfootball.net", DateTime.Today, DateTime.MinValue);
            }
            catch (Exception e)
            {
                // TODO?
                // This crappy site returns a 301 (and shows a 500) in case of a 404...
                logger.LogTrace($"{e.Message} article: {articleLabel} Url: {url}", e);
                return null;
            }
            return null;
        }

        private string GetSportsReference(string articleLabel, string sportsSiteName, DateTime deathDate)
        {
            // Multiple players going by the same name (and one deceased):
            // https://www.baseball-reference.com/search/search.fcgi?search=George+Smith        Died: June 15, 1987 
            // https://www.pro-football-reference.com/search/search.fcgi?search=George+Smith    Died: March 5, 1986
            // https://www.basketball-reference.com/search/search.fcgi?search=Eddie+Johnson     Died: October 26, 2020 
            // https://www.hockey-reference.com/search/search.fcgi?search=Brian+Smith           Died: August 2, 1995
            var searchUrl = "https://www." + sportsSiteName + "/search/search.fcgi?search=" + articleLabel.Replace(" ", "+");

            string response;

            try
            {
                response = webClient.DownloadString(searchUrl);
            }
            // TODO refactor
            catch (WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.InternalServerError)
                    throw new WebException(
                        $"ReferenceResolver.GetSportsReference site: {sportsSiteName}: Internal Server Error (500)",
                        innerException: new Exception("Server Error with Page Requested (500 error)\r\n" +
                            "We apologize, but there was an error in creating this page.\r\n" +
                            "- This could be for one of many reasons.\r\n" +
                            "- Our database is experiencing a very heavy load at this time.\r\n" +
                            "...\r\n" +
                            "- The hamsters powering our servers are taking a water break."));
                else
                    throw;
            }

            if (response.Contains("Found <strong>0 hits</strong> that match your search"))
                return null;

            // basketball-reference.com: no automatic redirect to players page in case of one match: player found via 'CheckMultiplePlayers'
            if (SportsPageContainsDateOfDeath(response, deathDate))
                return GetSportsWebReference(articleLabel, searchUrl, sportsSiteName);

            return CheckMultiplePlayers(response, articleLabel, sportsSiteName, deathDate);
        }

        private string GetSportsWebReference(string playerName, string url, string sportsSiteName)
        {
            return GenerateWebReference($"{playerName} Stats - {CapitalizeSportSiteName(sportsSiteName)}", url, sportsSiteName, DateTime.Today, DateTime.MinValue);
        }

        private string CapitalizeSportSiteName(string sportsSiteName)
        {
            var array = sportsSiteName.Split('-');

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].Substring(0, 1).ToUpper() + array[i].Substring(1));
                if (i < array.Length - 1)
                    sb.Append("-");
            }
            return sb.ToString();
        }

        private string CheckMultiplePlayers(string responseMultiplePlayers, string articleLabel, string sportsSiteName, DateTime deathDate)
        {
            Regex regex = new Regex(@"<a href=[\S]*players[\S]*>" + articleLabel);

            foreach (Match match in regex.Matches(responseMultiplePlayers))
            {
                var searchUrl = @"https://www." + sportsSiteName + GetPlayerId(match);
                var response = webClient.DownloadString(searchUrl);

                if (SportsPageContainsDateOfDeath(response, deathDate))
                    return GetSportsWebReference(articleLabel, searchUrl, sportsSiteName);
            }
            return null;
        }

        private bool SportsPageContainsDateOfDeath(string response, DateTime dateOfDeath)
        {
            var searchDoD = $"data-death=\"{(dateOfDeath.ToString("yyyy-MM-dd"))}\""; // f.i. // data-death="1996-05-26"
            return response.Contains(searchDoD);
        }

        private string GetReferenceFromWikidatRefItemsFirst(string articleLabel, List<string> referenceItems, DateTime deathDate)
        {
            if (!referenceItems.Any())
                return null;

            string url = null;

            // Resolve in preferred order of sources            
            if (BritannicaUrlFound(referenceItems, ref url))
                return GenerateWebReference(articleLabel, url, "britannica.com", DateTime.Today, DateTime.MinValue, publisher: "Encyclopædia Britannica Online");

            if (IndependentUrlFound(referenceItems, ref url))
            {
                var response = webClient.DownloadString(url);
                var deathDateSearch = GetLongEnDateText(deathDate);

                if (!response.Contains(deathDateSearch))
                    return $"Death date '{deathDateSearch}' not found HTML Independent";

                var data = RetrieveIndependentData(response);

                if (data == null)
                    return "JSON cannot be scraped from HTML Independent";

                var pubdate = DateTime.Parse(data.published_date);

                return GenerateNewsReference(data.article_title, url, "", DateTime.Today, "[[The Independent]]", pubdate, data.article_author);
            }

            if (IBDBUrlFound(referenceItems, ref url))
                return GenerateWebReference($"{articleLabel} - Broadway Cast & Staff - IBDB", url, "ibdb.com", DateTime.Today, DateTime.MinValue);

            if (DbeUrlFound(referenceItems, ref url))
                return GenerateWebReference($"{articleLabel} - DB~e", url, "dbe.rah.es", DateTime.Today, DateTime.MinValue, language: "es", publisher: "Real Academia de la Historia");

            if (BiografischPortaalUrlFound(referenceItems, ref url))
                return GenerateWebReference(articleLabel, url, "biografischportaal.nl", DateTime.Today, DateTime.MinValue, language: "nl");

            if (FemBioUrlFound(referenceItems, ref url))
                return GenerateWebReference("Frauendatenbank fembio.org", url, "fembio.org", DateTime.Today, DateTime.MinValue, language: "de");

            if (FilmportalUrlFound(referenceItems, ref url))
                return GenerateWebReference($"{articleLabel} - filmportal.de", url, "filmportal.de", DateTime.Today, DateTime.MinValue, language: "de");

            if (DecesUrlFound(referenceItems, ref url))
                return GenerateWebReference($"matchID - {articleLabel}", url, "[[Fichier des personnes décédées|Fichier des décès]]", DateTime.Today, DateTime.MinValue, language: "fr");

            return null;
        }

        private IndependentDigitalData RetrieveIndependentData(string response)
        {           
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var digitalDataNode = htmlDoc.DocumentNode.Descendants("amp-state")
            .Where(node => node.GetAttributeValue("id", "").Equals("digitalData")).FirstOrDefault();

            if (digitalDataNode == null)
                return null;

            var jsonString = digitalDataNode.FirstChild.InnerText;
            var data = JsonConvert.DeserializeObject<IndependentDigitalData>(jsonString);

            return data;
        }

        private string GetReferenceFromWikidatRefItemsSecond(string articleLabel, List<string> referenceItems)
        {
            if (!referenceItems.Any())
                return null;

            string url = null;
            string website = null;

            // Resolve in preferred order of sources                        

            if (LoCUrlFound(referenceItems, ref url))
                return GenerateWebReference($"{articleLabel} - Library of Congress", url, "id.loc.gov", DateTime.Today, DateTime.MinValue);

            if (SnacUrlFound(referenceItems, ref url))
                return GenerateWebReference($"{articleLabel} - Social Networks and Archival Context", url, "snaccooperative.org", DateTime.Today, DateTime.MinValue);

            if (BnFUrlFound(referenceItems, ref url, ref website))
                return GenerateWebReference(articleLabel, url, website, DateTime.Today, DateTime.MinValue, publisher: "Bibliothèque nationale de France", language: "French");

            return null;
        }

#pragma warning disable S1135 // Track uses of "TODO" tags
        // TODO create button to check websites to have not been checked already: 
#pragma warning restore S1135 // Track uses of "TODO" tags
        private bool BritannicaUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // https://www.britannica.com/biography/S-J-Perelman
            if (ReferenceUrlIdFound("Encyclopædia Britannica Online ID: biography/", @"https://www.britannica.com/biography/", referenceItems, true, ref referenceUrl))
                return true;

            // http://www.britannica.com/EBchecked/topic/18816/Viktor-Amazaspovich-Ambartsumian
            if (ReferenceUrlFound("http://www.britannica.com", referenceItems, true, ref referenceUrl))    // oud in Wikidata: http://
                return true;

            // https://www.britannica.com/biography/Donald-Alfred-Davie
            return ReferenceUrlFound("https://www.britannica.com", referenceItems, true, ref referenceUrl); // https://
        }

        private bool IndependentUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // e.g. https://www.independent.co.uk/news/people/obituary-stanley-woods-1488284.html
            // TODO true?
            return ReferenceUrlFound("https://www.independent.co.uk/news/people/", referenceItems, true, ref referenceUrl);
        }

        private bool BiografischPortaalUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // e.g. http://www.biografischportaal.nl/persoon/87547041  // nog steeds http:// https levert warning op (Chrome)
            return ReferenceUrlFound("http://www.biografischportaal.nl/persoon/", referenceItems, true, ref referenceUrl);
        }

        private bool IBDBUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // e.g. https://www.ibdb.com/broadway-cast-staff/tom-fuccello-88297
            return ReferenceUrlIdFound("Internet Broadway Database person ID: ", @"https://www.ibdb.com/broadway-cast-staff/", referenceItems, false, ref referenceUrl);
        }

        private bool DbeUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // e.g. https://dbe.rah.es/biografias/27266/xose-fernando-filgueira-valverde
            return ReferenceUrlIdFound("Spanish Biographical Dictionary ID: ", @"https://dbe.rah.es/biografias/", referenceItems, true, ref referenceUrl);
        }

        private bool FemBioUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // e.g. https://www.fembio.org/biographie.php/frau/frauendatenbank?fem_id=1560
            return ReferenceUrlIdFound("FemBio ID: ", @"https://www.fembio.org/biographie.php/frau/frauendatenbank?fem_id=", referenceItems, false, ref referenceUrl);
        }

        private bool FilmportalUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // e.g. https://www.filmportal.de/person/074db5fd4d494c69819053afc4222a5c
            return ReferenceUrlIdFound("Filmportal ID: ", @"https://www.filmportal.de/person/", referenceItems, true, ref referenceUrl);
        }

        private bool DecesUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // e.g. https://deces.matchid.io/id/Jwjy9PxtUQEm
            return ReferenceUrlFound("https://deces.matchid.io/id/", referenceItems, false, ref referenceUrl);
        }

        private bool LoCUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // e.g. https://id.loc.gov/authorities/names/n84163016.html
            return ReferenceUrlIdFound("Library of Congress authority ID: ", @"https://id.loc.gov/authorities/names/", referenceItems, true, ref referenceUrl);
        }

        private bool SnacUrlFound(List<string> referenceItems, ref string referenceUrl)
        {
            // https://snaccooperative.org/ark:/99166/w6xn23d4
            return ReferenceUrlIdFound("SNAC ARK ID: ", @"https://snaccooperative.org/ark:/99166/", referenceItems, false, ref referenceUrl);
        }

        private bool BnFUrlFound(List<string> referenceItems, ref string referenceUrl, ref string website)
        {
            // e.g http://data.bnf.fr/ark:/12148/cb13993433s     // oud in Wikidata: http://
            if (ReferenceUrlFound("http://data.bnf.fr/ark:/12148/", referenceItems, false, ref referenceUrl))  // false: request results in 303 response (redirect)
            {
                website = "data.bnf.fr";
                return true;
            }

            // e.g https://data.bnf.fr/ark:/12148/cb13993433s    //  https://
            if (ReferenceUrlFound("https://data.bnf.fr/ark:/12148/", referenceItems, false, ref referenceUrl)) // false: request results in 303 response (redirect)
            {
                website = "data.bnf.fr";
                return true;
            }

            // e.g. https://catalogue.bnf.fr/ark:/12148/cb12097177v
            //  Bibliothèque nationale de France ID: 12097177v
            foreach (var item in referenceItems)
            {
                const string idLabel = "Bibliothèque nationale de France ID: ";

                if (item.StartsWith(idLabel))
                {
                    var id = item.Substring(idLabel.Length);
                    referenceUrl = "https://catalogue.bnf.fr/ark:/12148/cb" + id; // do not check validity: request results in 303 response (redirect)
                    website = "catalogue.bnf.fr";
                    return true;
                }
            }

            return false;
        }

        private bool ReferenceUrlIdFound(string IdStartsWith, string urlStart, List<string> referenceItems, bool checkUrl, ref string referenceUrl)
        {
            foreach (var item in referenceItems)
            {
                if (item.StartsWith(IdStartsWith))
                {
                    var id = item.Substring(IdStartsWith.Length);
                    referenceUrl = urlStart + id;

                    if (checkUrl) // not all sites return a 404 in case of NotFound. See corr. tests
                        if (DownloadString(referenceUrl, false) == null)
                            return false;

                    return true;
                }
            }
            return false;
        }

        private bool ReferenceUrlFound(string urlStartsWith, List<string> referenceItems, bool checkUrl, ref string referenceUrl)
        {
            foreach (var item in referenceItems)
            {
                if (item.StartsWith($@"reference URL: {urlStartsWith}"))
                {
                    referenceUrl = item.Substring("reference URL: ".Length);
                    
                    if (checkUrl) // not all sites return a 404 in case of NotFound. See corr. tests
                        if (DownloadString(referenceUrl, false) == null)
                            return false;

                    return true;
                }
            }
            return false;
        }

        private string GetPlayerId(Match match)
        {
            return match.Value.ValueBetweenTwoStrings("\"", "\"");
        }

        private string GetLongEnDateText(DateTime date)
        {
            var ci = new CultureInfo("en-US");
            return date.ToString("d MMMM yyyy", ci);
        }

        private string GetWhere2GolfMonthName(DateTime date)
        {
            /*
            google site:where2golf.com "Died on"
                Died on Jan 6, 2016 in Tenerife
                Died on Feb 23, 1868 in Prestwick
                Died on March 21,
                Died on April 7, 1997 in Clearwater
                Died on May 7, 1968 in Palm Beach
                Died on June 19, 1904 in the Poorhouse
                Died on July 6, 1996
                Died on Aug 6, 1948 in Hackensack
                Died on Sept 11, 1968 in La
                Died on Oct 25, 1999
                Died on Nov 6, 2019 in Orange
                Died on Dec 26, 1916 in Mexico City
            */
            string monthName = date.ToString("MMMM", cultureInfo);

            if (monthName == "September")
                return "Sept";

            if (monthName.Length <= 5)
                return monthName;
            else
                return monthName.Substring(0, 3);
        }

        private string GenerateWebReference(string title, string url, string website, DateTime accessDate, DateTime date,
                                       string last1 = "", string first1 = "", string publisher = "", string language = "")
        {
            // <ref>{{cite web |last1=LASTNAME |first1=FIRSTNAME |title=TITLE |url=URL |website=britannica.com |publisher=[[The Scientist (magazine)|The Scientist]] |access-date=11 November 2021 |language=German |date=6 November 2000}}</ref>

            return "<ref>{{cite web" +
                    $" |last1={last1}" +
                    $" |first1={first1}" +
                    $" |title={title}" +
                    $" |url={url.Replace(@"\/", "/")}" + // unescape / (although never escaped)
                    $" |website={website}" +
                    $" |publisher={publisher}" +
                    $" |access-date={accessDate.ToString("d MMMM yyyy", cultureInfo)}" +
                    $" |language={language}" +
                    $" |date={(date == DateTime.MinValue ? string.Empty : date.ToString("d MMMM yyyy", cultureInfo))}" +
                   "}}</ref>";
        }

        private string GenerateNewsReference(string title, string url, string urlAccess, DateTime accessDate, string work, DateTime date,
                                       string author1 = "", string authorlink1 = "", string language = "")
        {
            return "<ref>{{cite news" +
                    $" |author1={author1}" +
                    $" |author-link1={authorlink1}" +
                    $" |title={title}" +
                    $" |url={url.Replace(@"\/", "/")}" + // unescape / (although never escaped)
                    $" |url-access={urlAccess}" +
                    $" |access-date={accessDate.ToString("d MMMM yyyy", cultureInfo)}" +
                    $" |work={work}" +
                    $" |language={language}" +
                    $" |date={(date == DateTime.MinValue ? string.Empty : date.ToString("d MMMM yyyy", cultureInfo))}" +
                   // $" |page={Page}" +
                   //$" |quote={Quote}" +
                   "}}</ref>";
        }

        public List<string> GetReferenceItems(string dateOfDeathReferences)
        {
            if (dateOfDeathReferences == null)
                return new List<string>();

            return dateOfDeathReferences.Split("~!").ToList();
        }

        private string DownloadString(string url, bool throwException)
        {
            try
            {
                var response = webClient.DownloadString(url);
                return response;
            }
            catch (WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                    return null;
                else
                {
                    if (throwException)
                        throw;

                    return null;
                }                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
