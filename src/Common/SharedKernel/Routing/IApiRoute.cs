namespace WeeControl.Common.SharedKernel.Routing
{
    public interface IApiRoute
    {
        string GetRoute(ApiRouteEnum api);
    }
}
