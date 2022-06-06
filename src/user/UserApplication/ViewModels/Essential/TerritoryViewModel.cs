using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Essential;

public class TerritoryViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly string uriString;
    private readonly TerritoryDto dto;
    
    public IEnumerable<TerritoryDto>? ListOfTerritories { get; private set; }

    
    public string TerritoryId
    {
        get => dto.TerritoryCode;
        set
        {
            dto.TerritoryCode = value;
            OnPropertyChanged(nameof(TerritoryId));
        }
    }
    
    public TerritoryViewModel(IDevice device) : base(device)
    {
        this.device = device;
        uriString = device.Server.GetFullAddress(Api.Essential.Territory.EndPoint);
        dto = new TerritoryDto();
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
                throw new ArgumentOutOfRangeException(response.StatusCode.ToString());
        }
    }

    public async Task AddOrUpdateTerritory(TerritoryDto dto)
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