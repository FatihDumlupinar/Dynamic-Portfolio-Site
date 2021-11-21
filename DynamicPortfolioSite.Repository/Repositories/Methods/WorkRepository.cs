using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Repository.Contexts;
using DynamicPortfolioSite.Repository.DataAccess.EntityFramework;
using DynamicPortfolioSite.Repository.Repositories.Interfaces;

namespace DynamicPortfolioSite.Repository.Repositories.Methods
{
    public class WorkRepository : EfCoreEntityRepository<Work>, IWorkRepository
    {
        public WorkRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
