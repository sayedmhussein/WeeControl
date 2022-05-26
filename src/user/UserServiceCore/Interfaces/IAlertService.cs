using WeeControl.User.UserServiceCore.Enums;

namespace WeeControl.User.UserServiceCore.Interfaces;

internal interface IAlertService
{
    Task DisplayAsync(AlertEnum alertEnum);
}