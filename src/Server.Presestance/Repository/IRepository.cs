using System;
using System.Collections.Generic;

namespace MySystem.Persistence.Infrastructure.EfRepository.Repository
{
    [Obsolete]
    public interface IRepository<TEntity>
    {
        TEntity Find(params object[] keyValues);
        IEnumerable<TEntity> FindAll();
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
