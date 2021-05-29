using System;
using Microsoft.AspNetCore.Authorization;

namespace MySystem.Web.Api.Security.Requirement
{
    public class SessionNotBlockedRequirement : IAuthorizationRequirement
    {
    }
}
