using DynamicPortfolioSite.Entities.Entities.Base;

namespace DynamicPortfolioSite.Entities.Entities
{
    public class AppUser : BaseEntity
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string UserImg { get; set; }
    }
}
