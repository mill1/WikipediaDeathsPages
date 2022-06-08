using System;
using System.Collections.Generic;
using System.Text;

namespace WikipediaDeathsPages.Service.Models
{
    public class IndependentDigitalData
    {
        public string page_url { get; set; }
        public string actual_url { get; set; }
        public string page_domain { get; set; }
        public string page_path { get; set; }
        public bool is_amp_page { get; set; }
        public string page_name { get; set; }
        public string page_type { get; set; }
        public string site_sections { get; set; }
        public string article_premium_status { get; set; }
        public string article_id { get; set; }
        public string article_title { get; set; }
        public string article_author { get; set; }
        public string article_category { get; set; }
        public string article_publication_time { get; set; }
        public string published_date { get; set; }
        public string first_published_date { get; set; }
        public string homepage_section { get; set; }
        public string article_page_filename { get; set; }
        public string lead_media_item { get; set; }
        public string article_topic_tags { get; set; }
        public int word_count { get; set; }
        public string content_source { get; set; }
        public string components_list { get; set; }
        public string fluctuating_components_list { get; set; }
        public int internal_links_count { get; set; }
        public int internal_topic_links_count { get; set; }
        public int internal_non_topic_links_count { get; set; }
        public string article_swap { get; set; }
    }
}
