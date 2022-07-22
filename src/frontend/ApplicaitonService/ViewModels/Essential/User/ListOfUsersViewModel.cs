using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Essential.User;

public class ListOfUsersViewModel : ViewModelBase
{
    private readonly IDevice device;
    
    public IEnumerable<UserDtoV1> ListOfUsers { get; private set; } = new List<UserDtoV1>();


    public ListOfUsersViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task GetListOfUsers()
    {
        var response = await SendMessageAsync<object>(
            new HttpRequestMessage
            {
                RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Route)),
                Version = new Version("1.0"),
                Method = HttpMethod.Get
            });

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<UserDtoV1>>>();
            ListOfUsers = content?.Payload ?? new List<UserDtoV1>();
            return;
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.BadGateway:
                await device.Alert.DisplayAlert("Check your internet connection");
                break;
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                await device.Alert.DisplayAlert("You are not authorized");
                break;
            default:
                await device.Alert.DisplayAlert("Unexpected Error Occured");
                throw new ArgumentOutOfRangeException();
        }
    }
}