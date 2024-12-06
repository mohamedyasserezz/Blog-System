using BlogSystem.Domain.Contract.Infrastructure;
using BlogSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure.GenericRepository
{
    public class GenericRepository<TEntity>(ApplicationDbContext _dbContext) : IGenricRepository<TEntity>
        where TEntity : class
    {
        #region Fields
        
        #endregion
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
        {
            return withTracking ?
                await _dbContext.Set<TEntity>().ToListAsync() :
                await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

       
   
    }
}
