using System;
using Microsoft.AspNetCore.Authorization;

namespace MySystem.Web.Domain.Security.Requirement
{
    public class SessionNotBlockedRequirement : IAuthorizationRequirement
    {
    }
}
