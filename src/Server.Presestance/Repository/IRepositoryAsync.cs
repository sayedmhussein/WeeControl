using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySystem.Persistence.Infrastructure.EfRepository.Repository
{
    [Obsolete]
    public interface IRepositoryAsync<TEntity>
    {
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
