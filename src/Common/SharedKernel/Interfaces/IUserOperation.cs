using WeeControl.Common.SharedKernel.Enums;

namespace WeeControl.Common.SharedKernel.Interfaces
{
    public interface IUserOperation
    {
        Task SaveAsync(UserDataEnum name, string value);

        Task<string> GetAsync(UserDataEnum name);
        
        Task ClearAsync();
        
        [Obsolete("Use SaveAsync(Token, ..")]
        Task SaveTokenAsync(string token);
        
        [Obsolete("Use GetAsync(Token)")]
        Task<string> GetTokenAsync();
    }
}