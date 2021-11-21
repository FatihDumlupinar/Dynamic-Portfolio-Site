using DynamicPortfolioSite.Entities.Entities.Base;

namespace DynamicPortfolioSite.Entities.Entities
{
    public class Education : BaseEntity
    {
        public string SchoolName { get; set; }
        public string Degree { get; set; }
        public string DateRange { get; set; }//Giriş ve Mezuniyet Örn : 2018 - 2020 
        public string Description { get; set; }

    }
}
