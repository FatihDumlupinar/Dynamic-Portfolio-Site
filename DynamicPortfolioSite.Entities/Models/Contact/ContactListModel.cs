using System;

namespace DynamicPortfolioSite.Entities.Models.Contact
{
    public class ContactListModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string SenderEmail { get; set; }
        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
