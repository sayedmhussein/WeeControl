﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.Security;

namespace WeeControl.WebApi.Services;

public class UserInfoService : ICurrentUserInfo
{
    private readonly IMediator mediatR;
    private Guid? sessionid = null;
    private readonly ICollection<string> territories = new List<string>();
        
    public UserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediatR)
    {
        Claims = httpContextAccessor?.HttpContext?.User.Claims;

        this.mediatR = mediatR;
    }

    public IEnumerable<Claim> Claims { get; private set; }

    public Guid? GetSessionId()
    {
        if (sessionid != null) return sessionid;

        var session_guid = Claims.FirstOrDefault(c => c.Type == ClaimsTagsList.Claims.SessionClaim)?.Value;
        if (Guid.TryParse(session_guid, out Guid session_string))
        {
            sessionid = session_string;
        }

        return sessionid;
    }

    public IEnumerable<Claim> GetClaimList()
    {
        return Claims;
    }

    public async Task<IEnumerable<string>> GetTerritoriesListAsync()
    {
        if (territories.Count != 0) return territories;
            
        var territoryCode = Claims.FirstOrDefault(c => c.Type == ClaimsTagsList.Claims.TerritoryClaim)?.Value;
        territories.Add(territoryCode);
        //var cla = await mediatR.Send(new GetTerritoriesQuery(territoryCode));

        // foreach (var bra in cla.Payload)
        // {
        //     territories.Add(bra.Id.ToString());
        // }

        return territories;
    }
}