using System.Net;
using System.Net.Http.Json;
using System.Transactions;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Admin;

public class AdminListOfUsersViewModel : ViewModelBase
{
    private readonly IDevice device;
    
    public IEnumerable<UserDtoV1> ListOfUsers { get; private set; } = new List<UserDtoV1>();


    public AdminListOfUsersViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task GetListOfUsers()
    {
        var response = await SendMessageAsync<object>(
            new HttpRequestMessage
            {
                RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Admin.User)),
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
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                await device.Alert.DisplayAlert("You are not authorized");
                break;
            default:
                await device.Alert.DisplayAlert("Unexpected Error Occured");
                throw new TransactionException();
                break;
        }
    }
}