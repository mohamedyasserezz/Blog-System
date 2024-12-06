using BlogSystem.Domain.Contract.Infrastructure;
using BlogSystem.Infrastructure.Data;
using BlogSystem.Infrastructure.GenericRepository;
using System.Collections.Concurrent;

namespace BlogSystem.Infrastructure.UnitOfWork
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        #region Fields

        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();

        #endregion

        #region Implementation of IUnitOfWork
        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();
        public ValueTask DisposeAsync() => _dbContext.DisposeAsync();

        public IGenricRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return (IGenricRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity>(_dbContext));
        }
        #endregion
    }
}
