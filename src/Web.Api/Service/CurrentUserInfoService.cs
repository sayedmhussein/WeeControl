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
        private Guid? sessionid;
        private IEnumerable<Guid> officeIds;
        

        public CurrentUserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediatR)
        {
            Claims = httpContextAccessor.HttpContext.User.Claims;

            this.mediatR = mediatR;
        }

        public Guid? SessionId
        {
            get
            {
                if (sessionid == null || sessionid == Guid.Empty)
                {
                    var sessionid_ = Claims.FirstOrDefault(c => c.Type == SharedKernel.Entities.Public.Constants.Claims.Types[SharedKernel.Entities.Public.Constants.Claims.ClaimType.Session])?.Value;
                    _ = Guid.TryParse(sessionid_, out Guid sessionId__);
                    sessionid = sessionId__;
                }

                return sessionid;
            }
        }

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
