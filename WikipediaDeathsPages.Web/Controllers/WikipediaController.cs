﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Wikimedia.Utilities.Dtos;
using WikipediaDeathsPages.Service.Dtos;
using WikipediaDeathsPages.Service.Interfaces;
using WikipediaDeathsPages.Service.Models;

namespace WikipediaDeathsPages.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WikipediaController : ControllerBase
    {
        private readonly ILogger<WikipediaController> logger;
        private readonly IWikipediaService wikipediaService;

        public WikipediaController(IWikipediaService wikipediaService, ILogger<WikipediaController> logger)
        {
            this.logger = logger;
            this.wikipediaService = wikipediaService;
        }

        [HttpGet("{deathDate}/{minimumScore:int}")]
        public DeathDateResultDto Get(DateTime deathDate, int minimumScore)
        {
            try
            {
                return wikipediaService.GetDeathDateResult(deathDate, minimumScore);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);

                string uri = "https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors";

                return new DeathDateResultDto
                {
                    Entries = new List<WikipediaListItemDto>
                    {
                        new WikipediaListItemDto
                        {
                            WikiText = e.Message + (e.InnerException == null ? string.Empty : "\r\nInner Exception: " + e.InnerException),
                            WikidataItem = new WikidataItemDto { DateOfBirth = DateTime.MinValue, DateOfDeath = DateTime.MinValue, Uri = uri, MannerOfDeath = "error: [WikipediaDeathsPages.Controllers.HttpGet] " + e.Message },
                            WikipediaArticle = new WikipediaArticleDto { DateOfBirth = DateTime.MinValue, DateOfDeath = DateTime.MinValue, Uri = uri }
                        }
                    }
                };
            }
        }        

        [HttpGet("score/{article}")]
        public ArticleMetrics GetNotabilityScore(string article)
        {
            return wikipediaService.GetArticleMetrics(article);
        }

        [HttpGet("scores")]
        public IEnumerable<ArticleMetrics> GetNotabilityScores()
        {
            /*
                Resolve notability of persons with death date 1 January of year to process.
                https://localhost:44304/wikipedia/scores
                Multiply metrics: https://www.howtogeek.com/775651/how-to-convert-a-json-file-to-microsoft-excel/ 
            */

            var articles = new List<string>
            {
                "Prince Eugen of Bavaria",
                "Aenne Brauksiepe",
                "Asnoldo Devonish",
                "Al Eugster",
                "Jean Feller",
                "Ivan Graziani",
                "Hagood Hardy",
                "Ham Harmon",
                "Mohammed Hafez Ismail",
                "Franc Joubin",
                "Graham Kersey",
                "Hans-Martin Majewski",
                "Jack Nissenthall",
                "James B. Pritchard",
                "Joan Rice",
                "Franco Volpi (actor)",
                "Harold Whalley",
                "Townes Van Zandt",
                "Ladislau Zilahi"
            };

            return wikipediaService.GetArticleMetrics(articles);
        }

        [HttpGet("reference/{article}/{deathDate}")]
        public string GetReferenceByKnownFor(string article, DateTime deathDate)
        {
            // https://localhost:44304/wikipedia/reference/Bob%20Paisley/1996-2-14
            // https://localhost:44304/wikipedia/reference/John%20Kramer%20(footballer)/1994-7-13
            return wikipediaService.ResolveReferenceByKnownFor(null, null, null, article, deathDate);
        }

        [HttpGet("references/{deathDate}")]
        public IEnumerable<string> GetReferencesByKnownFor(DateTime deathDate)
        {
            var references = new List<string>();

            /*
            Check for references regarding the existing entries and our new entries regarding January 1. 
            // 13-7-1994 (4 results!):
            // https://localhost:44304/wikipedia/references/1994-7-13
            var articles = new List<string>
            {
                "Eddie Boyd",
                "Anita Bärwirth",
                "Gerry Couture",
                "John Kramer (footballer)", 
                "Juozas Miltinis",
                "Jimmie Reese",
                "Murray Tyrrell",
                "Marik Vos-Lundh",
                "Olin Chaddock Wilson"
            };   
            */

            // 1-1-1997:
            // https://localhost:44304/wikipedia/references/1997-1-1
            var articles = new List<string>
            {
                // notable API entries:
                "Prince Eugen of Bavaria",
                "Al Eugster",
                "Jean Feller",
                "Franco Volpi (actor)",
                "Ladislau Zilahi",
                // Existing entries:
                 "Aenne Brauksiepe",
                "Asnoldo Devonish",
                "Ivan Graziani",
                "Hagood Hardy",
                "Mohammed Hafez Ismail",
                "Graham Kersey",
                "Hans-Martin Majewski",
                "James B. Pritchard",
                "Joan Rice",
                "Townes Van Zandt"
            };
            /*
             *Two results:
                "Jean Feller: <ref>{{cite web |last1= |first1= |title=Olympedia – Jean Feller |url=http://www.olympedia.org/athletes/26369 |website=olympedia.org |publisher=[[OlyMADMen]] |access-date=12 March 2022 |language= |date=}}</ref>",
                "Asnoldo Devonish: <ref>{{cite web |last1= |first1= |title=Olympedia – Asnoldo Devonish |url=http://www.olympedia.org/athletes/79265 |website=olympedia.org |publisher=[[OlyMADMen]] |access-date=12 March 2022 |language= |date=}}</ref>",
             */

            foreach (var article in articles)
            {
                var reference = wikipediaService.ResolveReferenceByKnownFor(null, null, null, article, deathDate);
                references.Add($"{article}: {reference}");
            }

            return references;
        }
    }
}
