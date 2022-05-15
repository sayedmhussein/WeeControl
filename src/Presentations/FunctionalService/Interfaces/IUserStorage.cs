using WeeControl.Presentations.FunctionalService.Enums;

namespace WeeControl.Presentations.FunctionalService.Interfaces;

public interface IUserStorage
{
    Task SaveAsync(UserDataEnum name, string value);

    Task<string> GetAsync(UserDataEnum name);
        
    Task ClearAsync();
}