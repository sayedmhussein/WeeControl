using WeeControl.Common.FunctionalService.Enums;

namespace WeeControl.Common.FunctionalService.Interfaces
{
    public interface IUserStorage
    {
        Task SaveAsync(UserDataEnum name, string value);

        Task<string> GetAsync(UserDataEnum name);
        
        Task ClearAsync();
    }
}