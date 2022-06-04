using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Essential.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.User;

public class RegisterViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly IRegisterDtoV1 dto;

    [Required]
    [StringLength(50)]
    [DisplayName("First Name")]
    public string FirstName
    {
        get => dto.FirstName;
        set
        {
            dto.FirstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    [Required]
    [StringLength(50)]
    [DisplayName("Last Name")]
    public string LastName
    {
        get => dto.LastName;
        set
        {
            dto.LastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email
    {
        get => dto.Email;
        set
        {
            dto.Email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    [Required]
    [MinLength(3)]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    [DisplayName("Username")]
    public string Username
    {
        get => dto.Username;
        set
        {
            dto.Username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password
    {
        get => dto.Password;
        set
        {
            dto.Password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    [Phone]
    [DisplayName("Mobile Number")]
    public string MobileNo
    {
        get => dto.MobileNo;
        set
        {
            dto.MobileNo = value;
            OnPropertyChanged(nameof(MobileNo));
        }
    }

    [Required]
    [DisplayName("Territory")]
    public string Territory
    {
        get => dto.TerritoryId;
        set
        {
            dto.TerritoryId = value;
            OnPropertyChanged(nameof(Territory));
        }
    }

    public RegisterViewModel(IDevice device) : base(device)
    {
        this.device = device;
        dto = new RegisterDtoV1();
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
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Base)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };

        var response = await SendMessageAsync(message, dto);

        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
            var token = responseDto?.Payload?.Token;
            await device.Security.UpdateTokenAsync(token ?? string.Empty);
            await device.Navigation.NavigateToAsync(Pages.Home.IndexPage, forceLoad: true);
            return;
        }
        
        switch (response.StatusCode)
        {
            case HttpStatusCode.Conflict:
                await device.Alert.DisplayAlert("Either username or email or mobile number already exist!");
                break;
            case HttpStatusCode.BadRequest:
                await device.Alert.DisplayAlert("Invalid details, please try again.");
                break;
            default:
                throw new ArgumentOutOfRangeException(response.StatusCode.ToString());
        }
    }
}