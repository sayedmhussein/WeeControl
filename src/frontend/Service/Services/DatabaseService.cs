using SQLite;
using WeeControl.Common.SharedKernel.Contexts.Business;
using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.AppService.Services;

internal class DatabaseService : IDatabaseService
{
    private readonly SQLiteAsyncConnection database;

    public DatabaseService(IDeviceStorage deviceStorage) //FileSystem.AppDataDirectory
    {
        if (database is null)
        {
            const SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite |
                                          SQLiteOpenFlags.Create |
                                          SQLiteOpenFlags.SharedCache;
            
            database = new SQLiteAsyncConnection(Path.Combine(deviceStorage.LocalStorageLocation, "AppDb.db3"), flags);
        }
    }
    
    private async Task Init<T>() where T : new()
    {
        var result = await database.CreateTableAsync<T>();
    }

    public async Task ClearTable<T>() where T : new()
    {
        await Init<T>();
        await database.DeleteAllAsync<T>();
    }

    public async Task AddToTable<T>(IEnumerable<T> data) where T : new()
    {
        await Init<T>();
        await database.InsertAsync(data);
    }

    public async Task AddToTable<T>(T item) where T : new()
    {
        await Init<T>();
        await database.InsertAsync(item);
    }

    public async Task<IEnumerable<T>> ReadFromTable<T>() where T : new()
    {
        await Init<T>();
        return await database.Table<T>().ToListAsync();
    }
}