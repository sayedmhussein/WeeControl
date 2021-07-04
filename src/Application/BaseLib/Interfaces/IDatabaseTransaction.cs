using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IDatabaseTransaction
    {
        Task InitAsync<T>() where T : new();

        Task<T> GetAsync<T>(Guid id) where T : new();

        Task<List<T>> GetAllAsync<T>() where T : new();

        Task<int> SaveAsync<T>(T entity) where T : new();

        Task<int> InsertAsync<T>(IEnumerable<T> entities) where T : new();

        Task<int> DeleteAsync<T>(T entity) where T : new();

        Task<int> DeleteAllAsync<T>() where T : new();
    }
}