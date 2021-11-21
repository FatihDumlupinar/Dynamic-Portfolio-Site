using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Repository.Contexts;
using DynamicPortfolioSite.Repository.DataAccess.EntityFramework;
using DynamicPortfolioSite.Repository.Repositories.Interfaces;

namespace DynamicPortfolioSite.Repository.Repositories.Methods
{
    public class SkillRepository : EfCoreEntityRepository<Skill>, ISkillRepository
    {
        public SkillRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
