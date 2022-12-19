namespace WeeControl.Frontend.AppService.Internals.Interfaces;

internal interface IDatabaseService
{
    Task ClearAllTables();
    Task ClearTable<T>() where T : new();
    Task AddToTable<T>(IEnumerable<T> data) where T : new();
    Task AddToTable<T>(T item) where T : new();
    Task<IEnumerable<T>> ReadFromTable<T>() where T : new();
}