using System.Net;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.ExtensionMethods;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Contexts.Essentials.Services;

internal class AuthenticationService : IAuthenticationService
{
    private readonly IGui gui;
    private readonly IDeviceSecurity security;
    private readonly IServerOperation server;
    private readonly IStorage storage;

    public AuthenticationService(IGui gui, IServerOperation server, IDeviceSecurity security, IStorage storage)
    {
        this.gui = gui;
        this.server = server;
        this.security = security;
        this.storage = storage;
    }

    public async Task Register(UserProfileDto dto)
    {
        if (!dto.Person.IsValidEntityModel())
        {
            await gui.DisplayAlert(dto.Person.GetFirstValidationError());
            return;
        }

        if (!dto.User.IsValidEntityModel())
        {
            await gui.DisplayAlert(dto.User.GetFirstValidationError());
            return;
        }

        var response = await server
            .GetResponseMessage(HttpMethod.Post,
                new Version("1.0"), dto,
                ApiRouting.Essentials.User.Route);

        if (response.IsSuccessStatusCode)
        {
            var read = await server.ReadFromContent<TokenResponseDto>(response.Content);
            if (read != null && !string.IsNullOrEmpty(read.Token))
            {
                await security.UpdateToken(read.Token);
                await gui.NavigateTo(ApplicationPages.Essential.OtpPage);
                return;
            }

            await gui.DisplayAlert($"Unexpected Error:{response.StatusCode}");
            return;
        }

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            await gui.DisplayAlert("Please choose another email or username as what you entered already exist");
            return;
        }

        await gui.DisplayAlert($"Unexpected error: {response.StatusCode}");
    }

    public async Task Login(LoginRequestDto dto)
    {
        var errors = dto.GetModelValidationErrors();
        if (errors.Any())
        {
            await gui.DisplayAlert(errors.Keys.First());
            return;
        }

        var response = await server
            .GetResponseMessage(HttpMethod.Post, new Version("1.0"), dto, ApiRouting.Essentials.Session.Route);

        if (response.IsSuccessStatusCode)
        {
            var token = await server.ReadFromContent<TokenResponseDto>(response.Content);
            if (token?.Token is not null)
            {
                await security.UpdateToken(token.Token);
                await gui.NavigateTo(ApplicationPages.Essential.OtpPage);
                return;
            }
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                await gui.DisplayAlert("Invalid credentials, please try again.");
                break;
            case HttpStatusCode.Forbidden:
                await gui.DisplayAlert("Account is locked, please contact your admin.");
                break;
            default:
                await gui.DisplayAlert($"Unexpected Error, please try again: {response.StatusCode}");
                break;
        }
    }

    public async Task UpdateToken()
    {
        if (await server.RefreshToken()) return;

        await gui.DisplayAlert("Please login.");
        await gui.NavigateTo(ApplicationPages.Essential.LoginPage, true);
    }

    public async Task UpdateToken(string otp)
    {
        if (await security.IsAuthenticated())
        {
            var response = await server
                .GetResponseMessage(HttpMethod.Put, new Version("1.0"), new object(),
                    ApiRouting.Essentials.Session.Route,
                    query: new[] {"otp", otp});

            if (response.IsSuccessStatusCode)
            {
                var token = await server.ReadFromContent<TokenResponseDto>(response.Content);
                if (token?.Token is not null)
                {
                    await security.UpdateToken(token.Token);
                    await gui.NavigateTo(ApplicationPages.Essential.HomePage, true);
                    return;
                }
            }
        }

        await gui.DisplayAlert("Please login.");
        await gui.NavigateTo(ApplicationPages.Essential.LoginPage);
    }

    public async Task Logout()
    {
        await security.DeleteToken();
        await storage.ClearKeysValues();
        var response = await server.GetResponseMessage(
            HttpMethod.Delete, new Version("1.0"),
            ApiRouting.Essentials.Session.Route);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            await gui.DisplayAlert(
                "An issue was encountered while logging out, please report this case to the developer.");

        await gui.NavigateTo(ApplicationPages.Essential.LoginPage, true);
    }

    public async Task RequestPasswordReset(UserPasswordResetRequestDto dto)
    {
        if (dto.IsValidEntityModel() == false)
        {
            await gui.DisplayAlert("invalid data");
            return;
        }

        var response = await server
            .GetResponseMessage(HttpMethod.Post,
                new Version("1.0"), dto,
                ApiRouting.Essentials.User.Route,
                ApiRouting.Essentials.User.PasswordEndpoint);

        if (response.IsSuccessStatusCode)
        {
            await gui.DisplayAlert("Please check your inbox for more instructions");
            await gui.NavigateTo(ApplicationPages.Essential.HomePage);
        }
    }
}