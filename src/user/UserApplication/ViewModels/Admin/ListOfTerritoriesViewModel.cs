using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Admin;

public class ListOfTerritoriesViewModel : ViewModelBase
{
    private readonly IDevice device;
    public IEnumerable<TerritoryDto> ListOfTerritories { get; private set; } 
    
    public ListOfTerritoriesViewModel(IDevice device) : base(device)
    {
        this.device = device;
        ListOfTerritories = new List<TerritoryDto>();
    }

    public async Task GetListOfTerritories()
    {
        var response = await SendMessageAsync<object>(
            new HttpRequestMessage
            {
                RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Territory.EndPoint)),
                Version = new Version("1.0"),
                Method = HttpMethod.Get
            });
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<TerritoryDto>>>();
            ListOfTerritories = content?.Payload ?? new List<TerritoryDto>();
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
                throw new ArgumentOutOfRangeException(response.RequestMessage.ToString());
        }
    }
}