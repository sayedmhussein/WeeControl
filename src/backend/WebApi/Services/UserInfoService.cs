﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.Common.SharedKernel;

namespace WeeControl.ApiApp.WebApi.Services;

public class UserInfoService : ICurrentUserInfo
{
    public IEnumerable<Claim> Claims { get; }
    public Guid? SessionId { get; }
    
    public string CountryId { get; }
    
    public UserInfoService(IHttpContextAccessor httpContextAccessor)
    {
        Claims = httpContextAccessor?.HttpContext?.User.Claims;
        
        var idStr = Claims?.FirstOrDefault(c => c.Type == ClaimsValues.ClaimTypes.Session)?.Value;
        if (Guid.TryParse(idStr, out var idGuid))
        {
            SessionId = idGuid;
        }

        CountryId = Claims?.FirstOrDefault(c => c.Type == ClaimsValues.ClaimTypes.Country)?.Value;
    }
}