using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IAppDatabase
    {
        Task<List<T>> GetAsync<T>() where T : new();
        Task<T> GetAsync<T>(Guid id) where T : new();
        Task<int> SaveAsync<T>(T entity) where T : new();
        Task<int> DeleteAsync<T>(T entity) where T : new();

        Task InitalizeTerritory();
    }
}