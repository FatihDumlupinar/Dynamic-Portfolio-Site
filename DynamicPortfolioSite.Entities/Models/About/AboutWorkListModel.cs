namespace DynamicPortfolioSite.Entities.Models.About
{
    public class AboutWorkListModel
    {
        public int WorkId { get; set; }//Eğer 0 ise yeni eklenmiştir
        public string JobName { get; set; }
        public string CompanyName { get; set; }
        public string DateRange { get; set; }// giriş ve çıkış tarihi
        public string Description { get; set; }
        public int RowNumber { get; set; }
    }
}
