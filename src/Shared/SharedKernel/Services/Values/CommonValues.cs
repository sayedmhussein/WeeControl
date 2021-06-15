using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using MySystem.SharedKernel.Enumerators.Common;
using MySystem.SharedKernel.Interfaces.Values;

namespace MySystem.SharedKernel.Services
{
    public class CommonValues : ICommonValues
    {
        public ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; private set; }

        public CommonValues()
        {
            dynamic obj = new EmbeddedResourcesManager(
                Assembly.GetExecutingAssembly())
                .GetSerializedAsJson("MySystem.SharedKernel.Configuration.appsettings.json");

            var apiRoute = new Dictionary<ApiRouteEnum, string>();
            foreach (var e in Enum.GetValues(typeof(ApiRouteEnum)).Cast<ApiRouteEnum>())
            {
                string value = obj.ApiRoute[e.ToString()];
                apiRoute.Add(e, value);
            }
            ApiRoute = apiRoute.ToImmutableDictionary();
        }
    }
}
