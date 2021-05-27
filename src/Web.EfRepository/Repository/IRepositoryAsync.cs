using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySystem.Web.Infrastructure.EfRepository.Repository
{
    public interface IRepositoryAsync<TEntity>
    {
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
