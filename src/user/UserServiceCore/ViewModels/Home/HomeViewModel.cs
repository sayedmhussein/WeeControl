using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Home;

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