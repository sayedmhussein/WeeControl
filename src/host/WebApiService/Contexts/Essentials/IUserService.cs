using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IUserService
{
    Task EditUserProfile(UserProfileUpdateDto dto);
    Task AddUser(UserProfileDto dto);

    Task<UserProfileDto> GetUserProfile();
    Task EditUser(object dto);
    Task ChangePassword(UserPasswordChangeRequestDto dto);
}