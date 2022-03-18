﻿using System.Threading.Tasks;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.Interfaces;

namespace WeeControl.Common.BoundedContext.Credentials.Operations
{
    public interface IUserOperation
    {
        Task<IResponseDto<TokenDto>> RegisterAsync(RegisterDto loginDto);

        Task<IResponseDto<TokenDto>> LoginAsync(LoginDto loginDto);
        Task<IResponseDto<TokenDto>> GetTokenAsync();
        Task<IResponseDto> LogoutAsync();

        
        Task<IResponseDto> UpdateEmailAsync(UpdateEmailAsync loginDto);
        Task<IResponseDto> UpdatePasswordAsync(UpdatePasswordDto loginDto);
        Task<IRequestDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    }
}