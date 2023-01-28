using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace WeeControl.Core.Application.Interfaces;

/// <summary>
/// Use it to get the current user's (requester) session-id, claims and territories.
/// </summary>
public interface ICurrentUserInfo
{
    IEnumerable<Claim> Claims { get; }
    Guid? SessionId { get; }
    string CountryId { get; }
}