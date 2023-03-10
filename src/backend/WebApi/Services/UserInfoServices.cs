using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.SharedKernel;

namespace WeeControl.Host.WebApi.Services;

public static class UserInfoServices
{
    public static IServiceCollection AddUserInfo(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserInfo, UserInfoService>();
        return services;
    }

    public class UserInfoService : ICurrentUserInfo
    {
        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            Claims = httpContextAccessor?.HttpContext?.User.Claims;

            var idStr = Claims?.FirstOrDefault(c => c.Type == ClaimsValues.ClaimTypes.Session)?.Value;
            if (Guid.TryParse(idStr, out var idGuid)) SessionId = idGuid;

            CountryId = Claims?.FirstOrDefault(c => c.Type == ClaimsValues.ClaimTypes.Country)?.Value;
        }

        public IEnumerable<Claim> Claims { get; }
        public Guid? SessionId { get; }

        public string CountryId { get; }
    }
}