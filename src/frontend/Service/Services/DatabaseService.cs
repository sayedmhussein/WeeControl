using SQLite;
using WeeControl.Common.SharedKernel.Contexts.Business;
using WeeControl.Frontend.AppService.Interfaces;

namespace WeeControl.Frontend.AppService.Services;

public class DatabaseService : IDatabaseService
{
    private SQLiteAsyncConnection Database;

    public DatabaseService(string appDataDirectory) //FileSystem.AppDataDirectory
    {
        if (Database is null)
        {
            const SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite |
                                          SQLiteOpenFlags.Create |
                                          SQLiteOpenFlags.SharedCache;
            
            Database = new SQLiteAsyncConnection(Path.Combine(appDataDirectory, "AppDb.db3"), flags);
        }
    }
    
    private async Task Init<T>() where T : new()
    {
        var result = await Database.CreateTableAsync<T>();
    }

    public async Task ClearTable<T>() where T : new()
    {
        await Init<T>();
        await Database.DeleteAllAsync<T>();
    }

    public async Task AddToTable<T>(IEnumerable<T> data) where T : new()
    {
        await Init<T>();
        await Database.InsertAsync(data);
    }

    public async Task AddToTable<T>(T item) where T : new()
    {
        await Init<T>();
        await Database.InsertAsync(item);
    }
}