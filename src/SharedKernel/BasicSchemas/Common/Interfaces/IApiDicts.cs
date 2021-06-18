using System.Collections.Immutable;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;

namespace WeeControl.SharedKernel.BasicSchemas.Common.Interfaces
{
    public interface IApiDicts
    {
        ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; }
    }
}
