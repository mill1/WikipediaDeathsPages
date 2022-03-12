using System;
using System.Collections.Generic;
using WikipediaDeathsPages.Service.Dtos;
using WikipediaDeathsPages.Service.Models;

namespace WikipediaDeathsPages.Service.Interfaces
{
    public interface IWikipediaService
    {
        IEnumerable<ArticleMetrics> GetArticleMetrics(IEnumerable<string> articles);
        ArticleMetrics GetArticleMetrics(string article);
        DeathDateResultDto GetDeathDateResult(DateTime deathDate, int minimumScore);
        string ResolveReferenceByKnownFor(ExistingEntryDto existingEntry, string dateOfDeathReferences, string wikiText, string articleLabel, DateTime deathDate);
    }
}
