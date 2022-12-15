namespace WeeControl.Frontend.AppService.Interfaces;

internal interface IDatabaseService
{
    Task ClearTable<T>() where T : new();
    Task AddToTable<T>(IEnumerable<T> data) where T : new();
    Task AddToTable<T>(T item) where T : new();
    Task<IEnumerable<T>> ReadFromTable<T>() where T : new();
}