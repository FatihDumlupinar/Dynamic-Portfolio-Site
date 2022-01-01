using System;

namespace DynamicPortfolioSite.Entities.Models.User
{
    public class UserListModel
    {
        public int UserId { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }

        public string CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? UpdatedByUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
