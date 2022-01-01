namespace DynamicPortfolioSite.Entities.Models.About
{
    public class AboutEducationListModel
    {
        public int EducationId { get; set; }
        public string SchoolName { get; set; }
        public string Degree { get; set; }
        public string DateRange { get; set; }//Giriş ve Mezuniyet Örn : 2018 - 2020 
        public string Description { get; set; }
        public int RowNumber { get; set; }
    }
}
