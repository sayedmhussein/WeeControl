using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Admin;

public class AdminViewModel : ViewModelBase
{
    private readonly IDevice device;
    
    public IEnumerable<UserDtoV1> ListOfUsers { get; private set; } = new List<UserDtoV1>();


    public AdminViewModel(IDevice device) : base(device)
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
            }, null);

        if (response.IsSuccessStatusCode)
        {
            ListOfUsers = await response.Content.ReadFromJsonAsync<IEnumerable<UserDtoV1>>() ?? new List<UserDtoV1>();
            return;
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                await device.Alert.DisplayAlert("You are not authorized");
                break;
        }
    }
}