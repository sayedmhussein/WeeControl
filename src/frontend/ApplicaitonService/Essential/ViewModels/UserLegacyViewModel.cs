using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using WeeControl.Frontend.ApplicationService.Essential.Legacy;
using WeeControl.Frontend.ApplicationService.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

[Obsolete]
public class UserLegacyViewModel : LegacyViewModelBase
{
    private readonly IDevice device;
    public readonly UserRegisterModel dto;
    
    public string FirstName
    {
        get => dto.FirstName;
        set
        {
            dto.FirstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    
    public string LastName
    {
        get => dto.LastName;
        set
        {
            dto.LastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    
    public string Email
    {
        get => dto.Email;
        set
        {
            dto.Email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    
    public string Username
    {
        get => dto.Username;
        set
        {
            dto.Username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    
    public string Password
    {
        get => dto.Password;
        set
        {
            dto.Password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    
    public string MobileNo
    {
        get => dto.MobileNo;
        set
        {
            dto.MobileNo = value;
            OnPropertyChanged(nameof(MobileNo));
        }
    }

    
    public string Territory
    {
        get => dto.TerritoryId;
        set
        {
            dto.TerritoryId = value;
            OnPropertyChanged(nameof(Territory));
        }
    }
    
    
    public string Nationality
    {
        get => dto.Nationality;
        set
        {
            dto.Nationality = value;
            OnPropertyChanged(nameof(Nationality));
        }
    }

    public UserLegacyViewModel(IDevice device, UserRegisterModel? dto = null) : base(device)
    {
        this.device = device;
        this.dto = dto ?? new UserRegisterModel();
    }

    public async Task RegisterAsync()
    {
        if (Validate(this, out var results)) 
        {
            IsLoading = true;
            await ProcessRegister();
            IsLoading = false;
            return;
        }
        
        await device.Alert.DisplayAlert(results.FirstOrDefault()?.ErrorMessage ?? "Invalid data entered!");
    }

    private async Task ProcessRegister()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Route)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };

        var response = await SendMessageAsync(message, dto);

        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
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
    }
}