using DynamicPortfolioSite.Entities.Entities.Base;

namespace DynamicPortfolioSite.Entities.Entities
{
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ProfileImg { get; set; }
        public string Url { get; set; }
    }
}
