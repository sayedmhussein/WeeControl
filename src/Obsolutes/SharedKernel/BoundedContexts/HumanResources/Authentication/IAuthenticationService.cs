using System;
using System.Threading.Tasks;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.Authentication
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