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
using MySystem.Web.Api.Service;
using MySystem.Web.Api.Security.Handler;
using MySystem.Web.Api.Security.Policy;
using MySystem.Web.Api.Domain.Employee;
using MySystem.Web.EfRepository;
using Web.Infrastructure.Repository.Employee;
using Sayed.MySystem.Api.Service;

namespace MySystem.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private DbContextOptions DbContextOptions
        {
            get
            {
                var builder = new DbContextOptionsBuilder<DataContext>();
                builder.UseNpgsql(Configuration.GetConnectionString("DbConnection"));
                builder.EnableSensitiveDataLogging();

                return builder.Options;
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddScoped<IEmployeeService>(provider => new EmployeeService(new EmployeeEfCore(DbContextOptions)));

            services.AddApiVersioning(ApiVersionService.ConfigureApiVersioning); //VersioningConfig(services);
            services.AddSwaggerGen(SwaggerService.ConfigureSwaggerGen);
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
                app.UseSwaggerUI(SwaggerService.ConfigureSwaggerUI);
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseMiddleware<UserInfoMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void VersioningConfig(IServiceCollection services)
        {
            //services.AddApiVersioning(options =>
            //{
            //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.ApiVersionReader = new MediaTypeApiVersionReader();
            //    options.ReportApiVersions = true;
            //});
        }

        private void AuthenticationConfig(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new JwtService(Configuration["Jwt:Key"]).ValidationParameters;

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
                options.AddPolicy(SessionNotBlockedPolicy.Name, SessionNotBlockedPolicy.Policy);
            });

            services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();
            services.AddScoped<IAuthorizationHandler, SessionNotBlockedHandler>();
        }
    }
}