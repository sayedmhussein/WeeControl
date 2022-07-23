using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.Frontend.ApplicationService.Essential.Legacy;

[Obsolete("Use HomeViewModel Init() function.")]
public class HomeLegacyViewModel : LegacyViewModelBase
{
    private readonly IDevice device;

    public HomeLegacyViewModel(IDevice device) : base(device)
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