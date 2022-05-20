namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

public interface IDeviceSecurity
{
    bool IsAuthenticated();

    void UpdateToken(string? token = null);
}