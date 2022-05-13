using WeeControl.Frontend.FunctionalService.Enums;

namespace WeeControl.Frontend.FunctionalService.Interfaces;

public interface IUserStorage
{
    Task SaveAsync(UserDataEnum name, string value);

    Task<string> GetAsync(UserDataEnum name);
        
    Task ClearAsync();
}