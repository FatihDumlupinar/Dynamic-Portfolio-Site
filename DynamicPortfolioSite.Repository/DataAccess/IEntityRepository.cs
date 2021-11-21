using Dapper;
using DynamicPortfolioSite.Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Repository.DataAccess
{
    public interface IEntityRepository<TEntity> where TEntity : BaseEntity, new()
    {
        #region Get

        TEntity Get(Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null);

        #endregion

        #region GetList

        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null);
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null);

        #endregion

        #region Update

        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);

        #endregion

        #region UpdateAll

        void UpdateAll(IEnumerable<TEntity> entity);
        Task UpdateAllAsync(IEnumerable<TEntity> entity);

        #endregion

        #region Add

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        Task<TEntity> AddAsyncReturnEntity(TEntity entity);

        #endregion

        #region AddAll

        void AddAll(IEnumerable<TEntity> entity);
        Task AddAllAsync(IEnumerable<TEntity> entity);

        #endregion

        #region Delete

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);

        #endregion

        #region Dapper
        
        Task<T> SpGetAsync<T>(string spName, DynamicParameters dynamicParameters = null);

        Task<List<T>> SpGetAllAsync<T>(string spName, DynamicParameters dynamicParameters = null); 

        #endregion

    }
}
