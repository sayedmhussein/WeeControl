using WeeControl.Common.SharedKernel.Enums;

namespace WeeControl.Common.SharedKernel.Interfaces
{
    public interface IUserStorage
    {
        Task SaveAsync(UserDataEnum name, string value);

        Task<string> GetAsync(UserDataEnum name);
        
        Task ClearAsync();
    }
}