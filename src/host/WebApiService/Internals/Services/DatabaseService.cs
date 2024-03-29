using SQLite;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class DatabaseService : IDatabaseService
{
    private readonly SQLiteAsyncConnection database;

    public DatabaseService(IStorage deviceStorage)
    {
        if (database is not null) return;

        const SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite |
                                      SQLiteOpenFlags.Create |
                                      SQLiteOpenFlags.SharedCache;

        Directory.CreateDirectory(deviceStorage.AppDataDirectory);

        database = string.IsNullOrEmpty(deviceStorage.AppDataDirectory)
            ? new SQLiteAsyncConnection(":memory:", flags)
            : new SQLiteAsyncConnection(Path.Combine(deviceStorage.AppDataDirectory, "AppDb.db3"), flags);
    }

    public async Task ClearAllTables()
    {
        try
        {
            var dbPath = database.DatabasePath;
            await database.CloseAsync();
            File.Delete(dbPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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

    private Task Init<T>() where T : new()
    {
        return database.CreateTableAsync<T>();
    }
}