using System;

namespace DynamicPortfolioSite.Entities.Models.Blog
{
    public class BlogModel
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ProfileImg { get; set; }
        public string Url { get; set; }
        public int LocalizationId { get; set; }

        public string CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
