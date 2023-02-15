using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class DatabaseService : IDatabaseService
{
    public Task ClearAllTables()
    {
        throw new NotImplementedException();
    }

    public Task ClearTable<T>() where T : new()
    {
        throw new NotImplementedException();
    }

    public Task AddToTable<T>(IEnumerable<T> data) where T : new()
    {
        throw new NotImplementedException();
    }

    public Task AddToTable<T>(T item) where T : new()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> ReadFromTable<T>() where T : new()
    {
        throw new NotImplementedException();
    }
}