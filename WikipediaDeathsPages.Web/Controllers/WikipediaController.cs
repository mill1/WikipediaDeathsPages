using Microsoft.AspNetCore.Mvc;
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
            // Once a year
            var articles = new List<string>
            {
                "Lionel Boulet",
                "Arleigh Burke",
                "Eddy Cobiness",
                "Dori Dorika",
                "Willie Hughes (footballer)",
                "N. Kannayiram",
                "Annie Lee Moss",
                "Fulvio Nesti",
                "Sergio Ojeda (boxer)",
                "Alifa Rifaat",
                "John Rodney",
                "Arthur Rudolph",
                "Eli Schechtman",
                "Jessie Vihrog",
                "Virgil W. Vogel",
                "Dave Woods (rugby league, born 1966)",
                "Sergei Yakovlev (actor)"
            };

            return wikipediaService.GetArticleMetrics(articles);
        }
    }
}
