using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.User;
using DynamicPortfolioSite.Repository.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Repository.Repositories.Interfaces
{
    public interface IAppUserRepository : IEntityRepository<AppUser>
    {
        Task<IEnumerable<AppUser>> CustomSearchAsync(UserSearchModel model);
    }
}
