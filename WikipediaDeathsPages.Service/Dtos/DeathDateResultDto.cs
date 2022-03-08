using System;
using System.Collections.Generic;
using Wikimedia.Utilities.Dtos;

namespace WikipediaDeathsPages.Service.Dtos
{
    public class DeathDateResultDto
    {
        public DateTime DateOfDeath { get; set; }
        public int ScoreNumberFour { get; set; }
        public IEnumerable<WikipediaListItemDto> Entries { get; set; }
        public IEnumerable<ExistingEntryDto> RejectedExistingEntries { get; set; }
    }
}
