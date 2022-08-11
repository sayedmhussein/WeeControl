using System.Net;
using System.Net.Http.Json;
using WeeControl.Frontend.ApplicationService.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class CustomerViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly IServerOperation server;

    public CustomerViewModel(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;
    }

    public async Task RegisterAsync(UserRegisterModel model)
    {
        IsLoading = true;
        
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Route)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };
        
        var dto = new RegisterCustomerDto();
        var response = await server.Send(message, dto);

        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<AuthenticationResponseDto>>();
            var token = responseDto?.Payload?.Token;
            await device.Security.UpdateTokenAsync(token ?? string.Empty);
            await device.Navigation.NavigateToAsync(Pages.Shared.IndexPage, forceLoad: true);
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
        IsLoading = false;
    }
}