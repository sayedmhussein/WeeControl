using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.Services;

internal class AlertService : IAlertService
{
    private readonly IDeviceAlert alert;

    public AlertService(IDevice device)
    {
        alert = device.Alert;
    }
    
    public Task DisplayAsync(AlertEnum alertEnum)
    {
        string str; 
        switch(alertEnum)
        {
            case AlertEnum.InvalidUsernameOrPassword:
                str = "Invalid Username or Password, please try again";
                break;
            default:
                str = Enum.GetName(typeof(AlertEnum), alertEnum) ?? throw new Exception("Enum not found exception");
                break;
        };

        return alert.DisplayAlert(str);
    }
}