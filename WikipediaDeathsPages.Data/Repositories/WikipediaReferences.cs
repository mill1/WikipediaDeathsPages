using System;
using System.Collections.Generic;
using System.Linq;
using WikipediaDeathsPages.Data.Interfaces;
using WikipediaDeathsPages.Data.Models;

namespace WikipediaDeathsPages.Data.Repositories
{
    public class WikipediaReferences : IWikipediaReferences
    {
        private List<SimpleOlympian> olympians = new List<SimpleOlympian>();

        public WikipediaReferences(WRContext context)
        {
            olympians = context.Olympians.Select(o => new SimpleOlympian { Id = o.Id, UsedName = o.UsedGivenName + " " + o.UsedSurname }).ToList();
        }

        public IList<int> GetIdsOfName(string name)
        {
            var Ids = olympians.Where(o => name.Equals(o.UsedName, StringComparison.OrdinalIgnoreCase)).Select(o => o.Id);

            return Ids.Any() ? Ids.ToList() : null;
        }
    }

    // TODO (maybe) remove: concatenate in table: (try) add column via Migration
    internal class SimpleOlympian
    {
        public int Id { get; set; }
        public string UsedName { get; set; }
    }

}
