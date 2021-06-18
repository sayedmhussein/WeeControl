using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WeeControl.Server.WebApi.StartupOptions
{
    public class SwaggerOptions
    {
        public static void ConfigureSwaggerGen(SwaggerGenOptions swaggerOptions)
        {
            swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MySystem.Api",
                Version = "v1",
                Description = "Api for MySystem Application to serve clients.",
                Contact = new OpenApiContact()
                {
                    Name = "Sayed M. Hussein",
                    Email = "Sayed.Hussein@gmx.com"
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT License"
                }
            });

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            swaggerOptions.IncludeXmlComments(xmlPath);

            swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        }

        public static void ConfigureSwagger(Swashbuckle.AspNetCore.Swagger.SwaggerOptions swaggerOptions)
        {
            swaggerOptions.RouteTemplate = "api-docs/{documentName}/swagger.json";
        }

        public static void ConfigureSwaggerUI(SwaggerUIOptions swaggerUIOptions)
        {
            swaggerUIOptions.SwaggerEndpoint($"/swagger/v1/swagger.json", "MySystem.Api v1");
            //swaggerUIOptions.RoutePrefix = "api-docs";
        }


    }
}
