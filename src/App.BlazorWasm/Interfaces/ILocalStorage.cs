using System.Threading.Tasks;

namespace WeeControl.App.BlazorWasm.Interfaces
{
    public interface ILocalStorage
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);

        Task ClearItems();
    }
}