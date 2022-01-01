namespace DynamicPortfolioSite.Entities.Models.Contact
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string SenderEmail { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }

    }
}
