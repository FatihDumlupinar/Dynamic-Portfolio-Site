using DynamicPortfolioSite.Entities.Entities.Base;

namespace DynamicPortfolioSite.Entities.Entities
{
    public class Work : BaseEntity
    {
        public string JobName { get; set; }
        public string CompanyName { get; set; }
        public string DateRange { get; set; }// giriş ve çıkış tarihi
        public string Description { get; set; }

    }
}
