namespace WeeControl.Frontend.AppService.Interfaces;

public interface IDatabaseService
{
    Task ClearTable<T>() where T : new();
    Task AddToTable<T>(IEnumerable<T> data) where T : new();
    Task AddToTable<T>(T item) where T : new();
}