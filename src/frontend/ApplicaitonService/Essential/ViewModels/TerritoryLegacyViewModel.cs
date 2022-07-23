using System.Net;
using System.Net.Http.Json;
using WeeControl.Frontend.ApplicationService.Essential.Legacy;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class TerritoryLegacyViewModel : LegacyViewModelBase
{
    private readonly IDevice device;
    private readonly string uriString;
    private readonly TerritoryModelDto modelDto;
    
    public IEnumerable<TerritoryModelDto>? ListOfTerritories { get; private set; }
    
    public string TerritoryId
    {
        get => modelDto.TerritoryCode;
        set
        {
            modelDto.TerritoryCode = value;
            OnPropertyChanged(nameof(TerritoryId));
        }
    }
    
    public TerritoryLegacyViewModel(IDevice device) : base(device)
    {
        this.device = device;
        uriString = device.Server.GetFullAddress(Api.Essential.Territory.EndPoint);
        modelDto = new TerritoryModelDto();
    }

    public async Task GetListOfTerritories()
    {
        var response = await SendMessageAsync<object>(
            new HttpRequestMessage
            {
                RequestUri = new Uri(uriString),
                Version = new Version("1.0"),
                Method = HttpMethod.Get
            });
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<TerritoryModelDto>>>();
            ListOfTerritories = content?.Payload ?? new List<TerritoryModelDto>();
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
        var response = await SendMessageAsync<object>(
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