using DynamicPortfolioSite.Entities.Entities.Base;

namespace DynamicPortfolioSite.Entities.Entities
{
    public class ProjectAndCategory : BaseEntity
    {
        public int ProjectId { get; set; }
        public int CategoryId { get; set; }
    }
}
