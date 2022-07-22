using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Essential.Home;

public class HomeViewModel : ViewModelBase
{
    private readonly IDevice device;

    public HomeViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public string NameOfUSer { get; private set; } = string.Empty;

    public async Task RefreshToken()
    {
        if (await RefreshTokenAsync())
        {
            NameOfUSer = await device.Storage.GetAsync(nameof(TokenDtoV1.FullName));
        }
    }
}