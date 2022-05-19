using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Application;
using WeeControl.Application.Interfaces;
using WeeControl.Infrastructure;
using WeeControl.Persistence;
using WeeControl.SharedKernel;
using WeeControl.WebApi.Middlewares;
using WeeControl.WebApi.Services;
using WeeControl.WebApi.StartupOptions;

namespace WeeControl.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddHttpContextAccessor();

        services.AddUserSecurityServiceForServer();
        services.AddApplication();
        services.AddInfrastructure(Configuration);
        
        _ = Configuration["UseInMemoryDb"] == false.ToString() ?
            services.AddPersistenceAsPostgres(Configuration, Assembly.GetExecutingAssembly().GetName().Name) :
            services.AddPersistenceAsInMemory();
        
        
        services.AddApiVersioning(ApiVersionOptions.ConfigureApiVersioning);
            
        services.AddScoped<ICurrentUserInfo, UserInfoService>();

        services.AddCors(c => c.AddPolicy("AllowAny", builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        }));

        services.AddAuthentication("Bearer").AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
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
            
        services.AddSwaggerGen(SwaggerOptions.ConfigureSwaggerGen);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(SwaggerOptions.ConfigureSwaggerUI);
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseCustomExceptionHandler();

        app.UseRouting();
            
        app.UseCors("AllowAny");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}