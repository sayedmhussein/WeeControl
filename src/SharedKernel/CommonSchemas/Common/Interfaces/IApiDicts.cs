using System.Collections.Immutable;
using WeeControl.SharedKernel.CommonSchemas.Common.Enums;

namespace WeeControl.SharedKernel.CommonSchemas.Common.Interfaces
{
    public interface IApiDicts
    {
        ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; }
    }
}
