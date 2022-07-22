using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.Frontend.ApplicationService.Essential.Home;

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