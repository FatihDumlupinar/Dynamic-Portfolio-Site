using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.Project;
using DynamicPortfolioSite.Repository.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Repository.Repositories.Interfaces
{
    public interface IProjectRepository : IEntityRepository<Project>
    {
        Task<IEnumerable<Project>> CustomSearchAsync(ProjectCustomSearchModel model);

    }
}
