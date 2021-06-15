using System;
using System.Collections.Immutable;
using MySystem.SharedKernel.Enumerators.Common;

namespace MySystem.SharedKernel.Interfaces.Values
{
    public interface ICommonValues
    {
        ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; }
    }
}
