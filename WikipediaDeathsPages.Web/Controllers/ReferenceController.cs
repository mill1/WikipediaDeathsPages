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
    public class ReferenceController : ControllerBase
    {
        private readonly ILogger<WikipediaController> logger;
        private readonly IReferenceService referenceService;

        public ReferenceController(IReferenceService referenceService, ILogger<WikipediaController> logger)
        {
            this.logger = logger;
            this.referenceService = referenceService;
        }

        [HttpGet("{article}/{deathDate}")]
        public string GetReference(string article, DateTime deathDate)
        {
            // https://localhost:44304/reference/Bob%20Paisley/1996-2-14
            // https://localhost:44304/reference/John%20Kramer%20(footballer)/1994-7-13
            return referenceService.Resolve(null, null, null, article, deathDate);
        }

        [HttpGet("{url}/{firstSearchPhrase}/{secondSearchPhrase}")]
        public bool CheckWebsite(string url, string firstSearchPhrase, string secondSearchPhrase)
        {
            // https://localhost:44304/reference/https:%2F%2Fwww.nu.nl/John%20Doe/1979-12-24
            return referenceService.CheckWebsite(url, new List<string> { firstSearchPhrase, secondSearchPhrase });
        }

        [HttpGet("list/{deathDate}")]
        public IEnumerable<string> GetReferences(DateTime deathDate)
        {
            var references = new List<string>();

            /*
            Check for references regarding the existing entries and our new entries regarding January 1.  
            */

            // 1-1-1997:
            // https://localhost:44304/reference/list/1997-1-1
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

            foreach (var article in articles)
            {
                var reference = referenceService.Resolve(null, null, null, article, deathDate);
                references.Add($"{article}: {reference}");
            }
            /*Two results:
                "Jean Feller: <ref>{{cite web |last1= |first1= |title=Olympedia – Jean Feller |url=http://www.olympedia.org/athletes/26369 |website=olympedia.org |publisher=[[OlyMADMen]] |access-date=12 March 2022 |language= |date=}}</ref>",
                "Asnoldo Devonish: <ref>{{cite web |last1= |first1= |title=Olympedia – Asnoldo Devonish |url=http://www.olympedia.org/athletes/79265 |website=olympedia.org |publisher=[[OlyMADMen]] |access-date=12 March 2022 |language= |date=}}</ref>",
             */

            return references;
        }
    }
}
