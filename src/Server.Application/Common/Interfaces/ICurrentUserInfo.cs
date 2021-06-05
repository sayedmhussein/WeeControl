using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MySystem.Application.Common.Interfaces
{
    public interface ICurrentUserInfo
    {
        Guid? SessionId { get; }

        IEnumerable<Guid> TerritoriesId { get; }

        IEnumerable<Claim> Claims { get; }
    }
}
