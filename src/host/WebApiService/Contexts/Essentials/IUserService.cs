using WeeControl.Core.DataTransferObject.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IUserService
{
    Task AddUser(UserProfileDto dto);
    Task EditUserProfile(UserProfileUpdateDto dto);
    Task<UserProfileDto> GetUserProfile();
    Task ChangePassword(UserPasswordChangeRequestDto dto);
}