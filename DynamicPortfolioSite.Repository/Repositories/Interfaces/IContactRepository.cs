using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.Contact;
using DynamicPortfolioSite.Repository.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Repository.Repositories.Interfaces
{
    public interface IContactRepository : IEntityRepository<Contact>
    {
        Task<IEnumerable<Contact>> CustomSearchAsync(ContactSearchModel model);

    }
}
