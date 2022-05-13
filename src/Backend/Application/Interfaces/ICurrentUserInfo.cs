using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WeeControl.Backend.Application.Interfaces;

/// <summary>
/// Use it to get the current user's (requester) session-id, claims and territories.
/// </summary>
public interface ICurrentUserInfo
{
    Guid? GetSessionId();

    IEnumerable<Claim> GetClaimList();
        
    Task<IEnumerable<string>> GetTerritoriesListAsync();
}