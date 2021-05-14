using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sayed.MySystem.Api.Helpers;
using Sayed.MySystem.Api.Policies;
using Sayed.MySystem.EntityFramework;

namespace Sayed.MySystem.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddDbContextPool<DataContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DbConnection"));
                options.EnableSensitiveDataLogging();
            });

            

            VersioningConfig(services);
            SwaggerConfig(services);
            AuthenticationConfig(services);
            AuthorizationConfig(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MySystem.Api v1"));
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void VersioningConfig(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.ReportApiVersions = true;
            });
        }

        private static void SwaggerConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MySystem.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            });
        }

        private void AuthenticationConfig(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //TryAllIssuerSigningKeys = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        try
                        {
                            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                            context.Token = token;
                        }
                        catch { }

                        return Task.CompletedTask;
                    },
                };
            });
        }

        private void AuthorizationConfig(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("LoggedIn", p =>
                {
                    p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    p.RequireClaim("sss");
                    p.RequireClaim("ofc");
                    p.RequireAssertion(context => Guid.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == "sss")?.Value, out Guid _));
                    p.RequireAssertion(context => Guid.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == "ofc")?.Value, out Guid _));
                });

                options.AddPolicy(HasRefreshedSession.Name, HasRefreshedSession.Policy);

                options.AddPolicy("CanEditEmployee", p =>
                {
                    p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    p.RequireClaim("isEmployee", "Admin", "HR");
                });
            });

            services.AddSingleton<IAuthorizationHandler, MaximumPeriodHandler>();
        }
    }
}