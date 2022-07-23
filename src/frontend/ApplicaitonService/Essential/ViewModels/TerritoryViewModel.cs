using System.Net;
using System.Net.Http.Json;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class TerritoryViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly IServerOperation server;
    private readonly string uriString;

    public List<TerritoryModelDto> ListOfTerritories { get; }
    
    public TerritoryViewModel(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;
        uriString = device.Server.GetFullAddress(Api.Essential.Territory.EndPoint);
        ListOfTerritories = new List<TerritoryModelDto>();
    }

    public async Task GetListOfTerritories()
    {
        var response = await server.Send<object>(
            new HttpRequestMessage
            {
                RequestUri = new Uri(uriString),
                Version = new Version("1.0"),
                Method = HttpMethod.Get
            });
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<TerritoryModelDto>>>();
            ListOfTerritories.Clear();
            var list = content?.Payload;
            if (list != null && list.Any())
            {
                ListOfTerritories.AddRange(list);
            }
            
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
                throw new ArgumentOutOfRangeException(response.StatusCode.ToString());
        }
    }

    public async Task AddOrUpdateTerritory(TerritoryModelDto modelDto)
    {
        var response = await server.Send<object>(
            new HttpRequestMessage
            {
                RequestUri = new Uri(uriString),
                Version = new Version("1.0"),
                Method = HttpMethod.Put
            });

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                await device.Alert.DisplayAlert("Saved");
                break;
            case HttpStatusCode.BadGateway:
                await device.Alert.DisplayAlert("Check your internet connection");
                break;
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                await device.Alert.DisplayAlert("You are not authorized");
                break;
            default:
                await device.Alert.DisplayAlert("Unexpected Error Occured");
                throw new ArgumentOutOfRangeException(response.StatusCode.ToString());
        }
    }
}