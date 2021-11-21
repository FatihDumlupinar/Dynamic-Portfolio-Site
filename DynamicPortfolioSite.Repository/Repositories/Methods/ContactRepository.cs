using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Repository.Contexts;
using DynamicPortfolioSite.Repository.DataAccess.EntityFramework;
using DynamicPortfolioSite.Repository.Repositories.Interfaces;

namespace DynamicPortfolioSite.Repository.Repositories.Methods
{
    public class ContactRepository : EfCoreEntityRepository<Contact>, IContactRepository
    {
        public ContactRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
