using System;
using System.Collections.Generic;
using WikipediaDeathsPages.Service.Dtos;

namespace WikipediaDeathsPages.Service.Interfaces
{
    public interface IReferenceService
    {
        string Resolve(DateTime deathDate, string dateOfDeathreferences, string articleLabel, string knownFor);
        string Resolve(string existingReference, DateTime deathDate, string dateOfDeathReferences, string articleLabel, string knownFor);
        List<string> GetReferenceItems(string dateOfDeathreferences);
        string Resolve(ExistingEntryDto existingEntry, string dateOfDeathReferences, string wikiText, string articleLabel, DateTime deathDate);
        bool CheckWebsite(string encodedUrl, string searchPhrases);
    }
}
