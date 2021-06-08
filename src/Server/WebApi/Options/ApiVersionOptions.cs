using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace MySystem.Web.Api.Options
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
