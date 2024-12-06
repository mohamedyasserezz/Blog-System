using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Domain.Contract.Infrastructure
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenricRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;
        Task<int> CompleteAsync();
    }
}
