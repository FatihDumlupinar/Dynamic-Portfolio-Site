using DynamicPortfolioSite.Entities.Entities.Base;

namespace DynamicPortfolioSite.Entities.Entities
{
    public class Contact : BaseEntity
    {
        public string Subject { get; set; }
        public string SenderEmail { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }

    }
}
