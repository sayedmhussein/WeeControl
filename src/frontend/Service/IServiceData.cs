namespace WeeControl.Frontend.AppService;

public interface IServiceData
{
    Task<bool> IsAuthenticated();
}