﻿using WeeControl.Core.DomainModel.Essentials.Dto;

namespace WeeControl.Host.WebApiService.Contexts.Essentials;

public interface IAuthenticationService
{
    Task Register(UserProfileDto dto);
    Task Login(LoginRequestDto dto);
    Task UpdateToken();
    Task UpdateToken(string otp);
    Task Logout();
    Task RequestPasswordReset(UserPasswordResetRequestDto dto);
}