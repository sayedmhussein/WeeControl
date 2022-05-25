namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDeviceSecurity
{
    Task<bool> IsAuthenticatedAsync();

    Task UpdateTokenAsync(string? token = null);
}