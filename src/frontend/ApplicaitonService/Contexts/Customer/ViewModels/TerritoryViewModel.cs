using System.Net;
using System.Net.Http.Json;
using WeeControl.Frontend.ApplicationService.Contexts.Customer.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Contexts.Customer.ViewModels;

public class TerritoryViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly IServerOperation server;
    private readonly string uriString;

    public List<TerritoryModel> ListOfTerritories { get; }
    
    public TerritoryViewModel(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;
        uriString = device.Server.GetFullAddress(Api.Essential.Routes.Territory);
        ListOfTerritories = new List<TerritoryModel>();
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
            var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<TerritoryDto>>>();
            ListOfTerritories.Clear();
            var list = content?.Payload;
            if (list != null && list.Any())
            {
                foreach (var t in list)
                {
                    ListOfTerritories.Add(new TerritoryModel(t));
                }
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

    public async Task AddOrUpdateTerritory(TerritoryModel modelDto)
    {
        var response = await server.Send(
            new HttpRequestMessage
            {
                RequestUri = new Uri(uriString),
                Version = new Version("1.0"),
                Method = HttpMethod.Put
            }, new TerritoryDto(null, modelDto));

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