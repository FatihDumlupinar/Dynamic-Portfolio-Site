using Dapper;
using DynamicPortfolioSite.Entities.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Repository.DataAccess.EntityFramework
{
    public class EfCoreEntityRepository<TEntity> : IEntityRepository<TEntity>
        where TEntity : BaseEntity, new()
    {
        #region Ctor&Fields

        protected readonly DbSet<TEntity> _entities;
        protected readonly DbContext _dbContext;

        public EfCoreEntityRepository(DbContext dbContext)
        {
            _entities = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }

        #endregion

        #region Add&AddAll

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddAll(IEnumerable<TEntity> entity)
        {
            _entities.AddRange(entity);
        }

        public async Task AddAllAsync(IEnumerable<TEntity> entity)
        {
            await _entities.AddRangeAsync(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task<TEntity> AddAsyncReturnEntity(TEntity entity)
        {
            await _entities.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddAllAsyncReturnEntities(IEnumerable<TEntity> entity)
        {
            await _entities.AddRangeAsync(entity);
            return entity;
        }

        #endregion

        #region Delete

        public void Delete(TEntity entity)
        {
            entity.IsActive = false;
            _entities.Update(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                entity.IsActive = false;
                _entities.Update(entity);
            });
        }

        #endregion

        #region Get&GetList
        
        public TEntity Get(Expression<Func<TEntity, bool>> filter = null)
        {
            var getData = _entities.AsNoTrackingWithIdentityResolution().Single(filter);
            return getData;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var getData = await _entities.AsNoTrackingWithIdentityResolution().SingleAsync(filter);
            return getData;
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            var getData = _entities.AsNoTrackingWithIdentityResolution().Where(filter).ToList();
            return getData;
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var getData = await _entities.AsNoTrackingWithIdentityResolution().Where(filter).ToListAsync();
            return getData;
        } 

        #endregion

        #region Update&UpdateAll

        public void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public void UpdateAll(IEnumerable<TEntity> entity)
        {
            _entities.UpdateRange(entity);
        }

        public async Task UpdateAllAsync(IEnumerable<TEntity> entity)
        {
            await Task.Run(() =>
            {
                _entities.UpdateRange(entity);
            });
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                _entities.Update(entity);
            });
        }

        #endregion

        #region Dapper

        public async Task<T> SpGetAsync<T>(string spName, DynamicParameters dynamicParameters = null)
        {
            var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            using var sqlcon = new NpgsqlConnection(connectionString);
            await sqlcon.OpenAsync();
            var response = await sqlcon.QuerySingleAsync<T>(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
            return response;
        }

        public async Task<List<T>> SpGetAllAsync<T>(string spName, DynamicParameters dynamicParameters = null)
        {
            var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;

            using var sqlcon = new NpgsqlConnection(connectionString);
            await sqlcon.OpenAsync();
            var response = await sqlcon.QueryAsync<T>(spName, dynamicParameters, commandType: CommandType.StoredProcedure);
            return response.ToList();
        }

        

        #endregion
    }
}
