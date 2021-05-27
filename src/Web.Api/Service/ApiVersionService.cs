using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Sayed.MySystem.Api.Service
{
    public class ApiVersionService
    {
        public static void ConfigureApiVersioning(ApiVersioningOptions options)
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new MediaTypeApiVersionReader();
            options.ReportApiVersions = true;
        }
    }
}
