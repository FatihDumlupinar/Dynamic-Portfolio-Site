using DynamicPortfolioSite.Entities.Entities.Base;

namespace DynamicPortfolioSite.Entities.Entities
{
    public class Skill : BaseEntity
    {
        public int AboutId { get; set; }

        public int RowNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
    }
}
