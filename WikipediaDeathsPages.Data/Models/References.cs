using System;

namespace WikipediaDeathsPages.Data.Models
{
    public partial class References
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string SourceCode { get; set; }
        public string ArticleTitle { get; set; }
        public string LastNameSubject { get; set; }
        public string Author1 { get; set; }
        public string Authorlink1 { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string UrlAccess { get; set; }
        public string Quote { get; set; }
        public string Work { get; set; }
        public string Agency { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string Location { get; set; }
        public DateTime AccessDate { get; set; }
        public DateTime Date { get; set; }
        public string Page { get; set; }
        public DateTime DeathDate { get; set; }
        public DateTime ArchiveDate { get; set; }

        public virtual Sources SourceCodeNavigation { get; set; }
    }
}
