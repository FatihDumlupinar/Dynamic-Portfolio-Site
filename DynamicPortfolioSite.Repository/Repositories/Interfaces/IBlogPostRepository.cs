using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.Blog;
using DynamicPortfolioSite.Repository.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Repository.Repositories.Interfaces
{
    public interface IBlogPostRepository : IEntityRepository<BlogPost>
    {
        Task<IEnumerable<BlogPost>> CustomSearchAsync(BlogCustomSearchModel model);

    }
}
