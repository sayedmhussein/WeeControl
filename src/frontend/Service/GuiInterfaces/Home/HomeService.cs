using System.Net;
using System.Net.Http.Json;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.Contexts.Authentication;
using WeeControl.Common.SharedKernel.Contexts.Home;
using WeeControl.Common.SharedKernel.Contexts.Temporary.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.AppService.Internals.Interfaces;
using WeeControl.Frontend.AppService.Models;

namespace WeeControl.Frontend.AppService.GuiInterfaces.Home;

internal class HomeService : IHomeService
{
    private readonly IDeviceData device;
    private readonly IServerOperation server;
    private readonly IDatabaseService db;
    private readonly IDeviceSecurity security;
    private const string UserRoute = ApiRouting.UserRoute;
    
    public List<MenuItemModel> MenuItems { get; }
    public IEnumerable<CountryModel> Countries { get; }
    public string GreetingMessage { get; private set; }

    public HomeService(IDeviceData device, IServerOperation server, IDatabaseService db, IDeviceSecurity security, IPersistedLists persistedLists)
    {
        this.device = device;
        this.server = server;
        this.db = db;
        this.security = security;
        Countries = persistedLists.Countries;
        MenuItems = new List<MenuItemModel>();
        GreetingMessage = "Hello";
    }
    
    public async Task<bool> VerifyAuthentication()
    {
        if ((await device.IsConnectedToInternet() == true && await server.RefreshToken()) ||
            (await device.IsConnectedToInternet() == false && await security.IsAuthenticatedAsync()))
        {
            await device.NavigateToAsync(ApplicationPages.HomePage);
            return true;
        }
        
        await device.NavigateToAsync(ApplicationPages.AuthenticationPage);
        return false;
    }

    public async Task<bool> Sync()
    {
        var claims = await security.GetClaimsPrincipal();
        if (claims.Claims.FirstOrDefault(x => x.Type == ClaimsValues.ClaimTypes.Territory)?.Value is not null)
        {
            //IsEmployee = true;
        }
            
        if (claims.Claims.FirstOrDefault(x => x.Type == ClaimsValues.ClaimTypes.Country)?.Value is not null)
        {
            //IsCustomer = true;
        }

        var response = await server.GetResponseMessage(HttpMethod.Get, new Version("1.0"), ApiRouting.UserHomeEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var dto = await server.ReadFromContent<HomeResponseDto>(response.Content);
            
        }

        return false;
    }

    public Task<string> GetGreetingMessage()
    {
        return Task.FromResult("Hello");
    }

    public Task<IEnumerable<HomeFeedModel>> GetHomeFeeds()
    {
        return db.ReadFromTable<HomeFeedModel>();
    }

    public Task<IEnumerable<HomeNotificationModel>> GetHomeNotifications()
    {
        return db.ReadFromTable<HomeNotificationModel>();
    }

    public async Task Register(RegisterCustomerDto registerModel)
    {
        await RegisterAsync(registerModel);
    }

    public async Task RequestPasswordReset(UserPasswordResetRequestDto passwordResetModel)
    {
        if (string.IsNullOrWhiteSpace(passwordResetModel.Email) || string.IsNullOrWhiteSpace(passwordResetModel.Username))
        {
            await device.DisplayAlert("Please enter your email and username first!");
            return;
        }
        
        await ProcessPasswordReset(passwordResetModel);
    }

    public async Task ChangeMyPassword(UserPasswordChangeRequestDto passwordChangeModel)
    {
        if (string.IsNullOrWhiteSpace(passwordChangeModel.OldPassword) ||
            string.IsNullOrWhiteSpace(passwordChangeModel.NewPassword) ||
            string.IsNullOrWhiteSpace(passwordChangeModel.ConfirmPassword) ||
            passwordChangeModel.NewPassword != passwordChangeModel.ConfirmPassword)
        {
            await device.DisplayAlert("Invalid Properties");
            return;
        }
        
        await ProcessChangingPassword(passwordChangeModel);
    }

    public string FeedsLabel => "Feeds";
    public string SyncButtonLabel => "Sync";
    public string InternetIssueMessage => "Unable to reach to server, please try again later.";

    private async Task ProcessPasswordReset(UserPasswordResetRequestDto dtoV1)
    {
        var s = new string[] { "", "" };
        var response = await server.GetResponseMessage(
            HttpMethod.Post, new Version("1.0"), new [] {
            ApiRouting.UserRoute, ApiRouting.UserPasswordEndpoint
        }, dtoV1);
        
        if (response.IsSuccessStatusCode)
        {
            await device.DisplayAlert("New Password was created, please check your email.");
            await device.NavigateToAsync(ApplicationPages.Essential.UserPage, forceLoad:true);
            return;
        }
        
        Console.WriteLine("Invalid message: " + response.ReasonPhrase);
        await device.DisplayAlert("Something went wrong!");
    }
    
    private async Task RegisterAsync(RegisterCustomerDto model)
    {
        var response = await server.GetResponseMessage(
            HttpMethod.Post, new Version("1.0"), ApiRouting.UserRoute, model);

        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenResponseDto>>();
            var token = responseDto?.Payload?.Token;
            await security.UpdateTokenAsync(token ?? string.Empty);
            await device.NavigateToAsync(ApplicationPages.SplashPage, forceLoad: true);
            return;
        }

        var displayString = response.StatusCode switch
        {
            HttpStatusCode.Conflict => "Either username or email or mobile number already exist!",
            HttpStatusCode.BadRequest => "Invalid details, please try again.",
            HttpStatusCode.BadGateway => "Please check your internet connection then try again.",
            _ => throw new ArgumentOutOfRangeException(response.StatusCode.ToString())
        };
        
        await device.DisplayAlert(displayString);
    }
    
    private async Task ProcessChangingPassword(UserPasswordChangeRequestDto dto)
    {
        var response = await server.GetResponseMessage(
            HttpMethod.Patch, new Version("1.0"), new [] {
                ApiRouting.UserRoute, ApiRouting.UserPasswordEndpoint }, dto);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.DisplayAlert("PasswordUpdatedSuccessfully");
                await device.NavigateToAsync(ApplicationPages.SplashPage);
                return;
            case HttpStatusCode.NotFound:
                await device.DisplayAlert("InvalidPassword");
                break;
            default:
                await device.DisplayAlert("DeveloperMinorBug");
                break;
        }
        
        await device.NavigateToAsync(ApplicationPages.Essential.UserPage, forceLoad: true);
    }
}