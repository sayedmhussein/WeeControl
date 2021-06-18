using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using WeeControl.SharedKernel.CommonSchemas.Common.Enums;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;
using WeeControl.SharedKernel.Configurations;

namespace WeeControl.SharedKernel.CommonSchemas.Common.Dicts
{
    public class ApiDicts : BaseDicts, IApiDicts
    {
        public ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; private set; }

        public ApiDicts() : base(Assembly.GetExecutingAssembly(), Settings.Filename)
        {
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
