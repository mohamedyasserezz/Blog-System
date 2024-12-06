namespace BlogSystem.Domain.Contract.Infrastructure
{
    public interface IGenricRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false);
        public Task<TEntity?> GetByIdAsync(int id);
        public Task AddAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);

    }
}
