using System.Net;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.ExtensionHelpers;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Contexts.Essentials.Services;

internal class AuthenticationService(IGui gui, IServerOperation server, IDeviceSecurity security, IStorage storage)
    : IAuthenticationService
{
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

    
}