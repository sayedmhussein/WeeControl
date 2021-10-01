using System;
using System.Threading.Tasks;
using WeeControl.SharedKernel.Authorization.DtosV1;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.App.Services.Authorization
{
    public interface IAuthenticationService
    {
        Task<IResponseDto> RequestNewToken(RequestNewTokenDto dto);
        
        Task<IResponseDto> RefreshCurrentToken();
        
        Task<IResponseDto> Logout();
        
        Task<IResponseDto> RequestPasswordReset(RequestPasswordResetDto dto);

        Task<IResponseDto> SetNewPassword(SetNewPasswordDto dto);
        
        public event EventHandler<string> TokenChanged;
    }
}