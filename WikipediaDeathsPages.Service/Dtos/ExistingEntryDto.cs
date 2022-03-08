using System;

namespace WikipediaDeathsPages.Service.Dtos
{
    public class ExistingEntryDto
    {
        public string ArticleName { get; set; }
        public string ArticleLinkedName { get; set; }
        public string Information { get; set; }
        public string Reference { get; set; }
        public string ReferenceUrl { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string Uri { get; set; }
        public string WikiText { get; set; }
        public int NotabilityScore { get; set; }
        public bool Keep { get; set; }
        public string ReasonKeepReject { get; set; }
    }
}
