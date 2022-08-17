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

        [HttpGet("articleanomalies/{year}/{monthId}")]
        public IEnumerable<ArticleAnomalieResultDto> GetArticleAnomalies(int year, int monthId)
        {
            // https://localhost:44304/wikipedia/articleanomalies/1999/4

            try
            {
                return wikipediaService.ResolveArticleAnomalies(year, monthId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                var exceptionName = e.GetType().Name;

                return new List<ArticleAnomalieResultDto>
                {
                    new ArticleAnomalieResultDto
                    {
                        ArticleLinkedName = $"Error: {exceptionName}",
                        Uri = $"https://www.google.com/search?q=site%3Astackoverflow.com+{exceptionName}",
                        Text = e.Message
                    }
                };
            }
        }

        [HttpGet("score/{article}")]
        public ArticleMetrics GetNotabilityScore(string article)
        {
            return wikipediaService.GetArticleMetrics(article);
        }

        [HttpGet("list/scores")]
        public IEnumerable<ArticleMetrics> GetNotabilityScores()
        {
            /*
                Resolve notability of persons with death date 1 January of year to process.
                https://localhost:44304/wikipedia/list/scores
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
    }
}
