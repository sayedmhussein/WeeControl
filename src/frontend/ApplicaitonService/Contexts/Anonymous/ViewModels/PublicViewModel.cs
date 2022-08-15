using System.Net;
using System.Net.Http.Json;
using WeeControl.Frontend.ApplicationService.Contexts.Anonymous.Interfaces;
using WeeControl.Frontend.ApplicationService.Contexts.Anonymous.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Contexts.Anonymous.ViewModels;

internal class PublicViewModel : ViewModelBase, IPublicViewModel
{
    private readonly IDevice device;
    private readonly IServerOperation server;

    public CustomerRegisterModel CustomerRegisterModel { get; }
    public PasswordResetModel PasswordResetModel { get; }
    public IEnumerable<CountryModel> Countries { get; }

    public PublicViewModel(IDevice device, IServerOperation server, IPersistedLists persistedLists)
    {
        this.device = device;
        this.server = server;

        CustomerRegisterModel = new CustomerRegisterModel();
        PasswordResetModel = new PasswordResetModel();
        Countries = persistedLists.Countries;
    }
    
    public Task<bool> UsernameAllowed()
    {
        throw new NotImplementedException();
    }

    public Task<bool> EmailAllowed()
    {
        throw new NotImplementedException();
    }

    public Task<bool> MobileNumberAllowed()
    {
        throw new NotImplementedException();
    }

    public async Task Register()
    {
        IsLoading = true;
        await RegisterAsync(CustomerRegisterModel);
        IsLoading = false;
    }

    public async Task RequestPasswordReset()
    {
        if (string.IsNullOrWhiteSpace(PasswordResetModel.Email) || string.IsNullOrWhiteSpace(PasswordResetModel.Username))
        {
            await device.Alert.DisplayAlert("You didn't entered proper data");
            return;
        }
        
        IsLoading = true;
        await ProcessPasswordReset(PasswordResetModel);
        IsLoading = false;
    }
    
    private async Task ProcessPasswordReset(UserPasswordResetRequestDto? dtoV1)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Customer)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };
        
        var responseMessage = await server.Send(message, dtoV1);
        if (responseMessage.IsSuccessStatusCode)
        {
            await device.Navigation.NavigateToAsync(Pages.Anonymous.LoginPage);
            await device.Alert.DisplayAlert("New Password was created, please check your email.");
            return;
        }
        
        Console.WriteLine("Invalid message: " + responseMessage.ReasonPhrase);
        await device.Alert.DisplayAlert("Something went wrong!");
    }
    
    private async Task RegisterAsync(CustomerRegisterModel model)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Customer)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };
        
        var dto = new RegisterCustomerDto();
        var response = await server.Send(message, model);

        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<AuthenticationResponseDto>>();
            var token = responseDto?.Payload?.Token;
            await device.Security.UpdateTokenAsync(token ?? string.Empty);
            await device.Navigation.NavigateToAsync(Pages.Anonymous.IndexPage, forceLoad: true);
            return;
        }

        var displayString = response.StatusCode switch
        {
            HttpStatusCode.Conflict => "Either username or email or mobile number already exist!",
            HttpStatusCode.BadRequest => "Invalid details, please try again.",
            HttpStatusCode.BadGateway => "Please check your internet connection then try again.",
            _ => throw new ArgumentOutOfRangeException(response.StatusCode.ToString())
        };
        
        await device.Alert.DisplayAlert(displayString);
    }
}