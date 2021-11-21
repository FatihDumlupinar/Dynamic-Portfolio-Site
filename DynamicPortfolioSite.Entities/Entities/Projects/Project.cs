using DynamicPortfolioSite.Entities.Entities.Base;

namespace DynamicPortfolioSite.Entities.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SrcUrl { get; set; }
        public string Img { get; set; }

    }
}
