﻿using System.Net;
using System.Threading.Tasks;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;

namespace WeeControl.Common.BoundedContext.Credentials
{
    public interface IUserOperation
    {
        Task RegisterAsync(RegisterDto loginDto);
        Task<(HttpStatusCode, string Token)> LoginAsync(LoginDto loginDto);
        Task GetTokenAsync();
        Task LogoutAsync();
        Task UpdateEmailAsync(UpdateEmailAsync loginDto);
        Task UpdatePasswordAsync(UpdatePasswordDto loginDto);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    }
}