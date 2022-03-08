using System.Collections.Generic;

namespace WikipediaDeathsPages.Data.Interfaces
{
    public interface IWikipediaReferences
    {
        IList<int> GetIdsOfName(string name);
    }
}
