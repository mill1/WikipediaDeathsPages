using System.Collections.Generic;

namespace WikipediaDeathsPages.Service.Interfaces
{
    public interface IReferenceService
    {
        string Resolve(System.DateTime deathDate, string dateOfDeathreferences, string articleLabel, string knownFor);
        List<string> GetReferenceItems(string dateOfDeathreferences);
    }
}
