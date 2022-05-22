namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDeviceSecurity
{
    bool IsAuthenticated();

    void UpdateToken(string? token = null);
}