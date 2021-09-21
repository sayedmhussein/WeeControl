using System.Threading.Tasks;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Authorization;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Frontend.CommonLib.DataAccess.Authentication
{
    public interface IAuthenticationService
    {
        Task<IResponseDto> RequestPasswordReset(RequestPasswordResetDto dto);

        Task<IResponseDto> SetNewPassword(SetNewPasswordDto dto);
        
        Task<IResponseDto> RequestNewToken(RequestNewTokenDto dto);
        
        Task<IResponseDto> RefreshCurrentToken();
        
        Task<IResponseDto> Logout();
    }
}