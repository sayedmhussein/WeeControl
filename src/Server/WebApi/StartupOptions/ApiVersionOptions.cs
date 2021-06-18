using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace WeeControl.Server.WebApi.StartupOptions
{
    public class ApiVersionOptions
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
