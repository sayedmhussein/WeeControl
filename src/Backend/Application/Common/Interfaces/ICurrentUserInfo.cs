using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace WeeControl.Backend.Application.Common.Interfaces
{
    public interface ICurrentUserInfo
    {
        Guid? SessionId { get; }

        IEnumerable<Guid> Territories { get; }

        IEnumerable<Claim> Claims { get; }
    }
}
