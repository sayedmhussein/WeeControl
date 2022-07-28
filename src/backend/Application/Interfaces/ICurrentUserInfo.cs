using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace WeeControl.Application.Interfaces;

/// <summary>
/// Use it to get the current user's (requester) session-id, claims and territories.
/// </summary>
public interface ICurrentUserInfo
{
    IEnumerable<Claim> Claims { get; }
    Guid? SessionId { get; }
    
    
    
    
    
    [Obsolete("Use property instead of function.")]
    Guid? GetSessionId();

    [Obsolete("Server will check for eligibility using mediator.")]
    Task<IEnumerable<string>> GetTerritoriesListAsync(CancellationToken cancellationToken);
}