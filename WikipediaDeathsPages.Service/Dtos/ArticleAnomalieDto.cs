using System;

namespace WikipediaDeathsPages.Service.Dtos
{
    public class ArticleAnomalieDto
    {
        public string ArticleLinkedName { get; set; }
        public string Uri { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string Text { get; set; }
    }
}
