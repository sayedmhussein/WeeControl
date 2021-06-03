using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Employee.Query.GetEmployeeTerritories.V1;

namespace MySystem.Api.Service
{
    public class CurrentUserInfoService : ICurrentUserInfo
    {
        private readonly IMediator mediatR;
        private IEnumerable<Guid> officeIds;

        public CurrentUserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediatR)
        {
            Claims = httpContextAccessor.HttpContext.User.Claims;

            var sessionid = Claims.FirstOrDefault(c => c.Type == SharedKernel.Entities.Public.Constants.Claims.Types[SharedKernel.Entities.Public.Constants.Claims.ClaimType.Session])?.Value;
            _ = Guid.TryParse(sessionid, out Guid SessionId);
            this.mediatR = mediatR;
        }

        public Guid? SessionId { get; private set; }

        public IEnumerable<Claim> Claims { get; private set; }

        public IEnumerable<Guid> TerritoriesId
        {
            get
            {
                if (officeIds == null)
                {
                    var bla = mediatR.Send(new GetEmployeeTerritoriesQuery() { SessionId = (Guid)SessionId }).GetAwaiter().GetResult()?.Payload;
                    officeIds = new List<Guid>();
                    foreach (var shit in bla)
                    {
                        officeIds.Append(shit.TerritoryId);
                    }
                }

                return officeIds;
            }
        }
    }
}
