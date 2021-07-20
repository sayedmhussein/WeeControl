using System;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;

namespace WeeControl.SharedKernel.BasicSchemas.Common
{
    public interface ICommonLists
    {
        string GetRoute(ApiRouteEnum api);
    }
}
