using System;

namespace WeeControl.SharedKernel.Routing
{
    public interface IApiRoute
    {
        string GetRoute(ApiRouteEnum api);
    }
}
