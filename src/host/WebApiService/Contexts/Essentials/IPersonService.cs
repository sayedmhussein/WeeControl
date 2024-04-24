using WeeControl.Core.DomainModel.Essentials.Dto;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IPersonService
{
    Task Register(UserProfileDto dto);
    Task RequestPasswordReset(UserPasswordResetRequestDto dto);
}