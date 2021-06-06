using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Territory.Query.GetTerritories;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Api.Service
{
    public class UserInfoService : ICurrentUserInfo
    {
        private readonly IMediator mediatR;
        private readonly IValuesService values;
        private Guid? sessionid;
        private IEnumerable<Guid> officeIds;
        

        public UserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediatR, IValuesService values)
        {
            Claims = httpContextAccessor.HttpContext.User.Claims;

            this.mediatR = mediatR;
            this.values = values;
        }

        public Guid? SessionId
        {
            get
            {
                if (sessionid == null || sessionid == Guid.Empty)
                {
                    var sessionid_ = Claims.FirstOrDefault(c => c.Type == values.ClaimType[ClaimTypeEnum.Session])?.Value;
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
                    var bla = mediatR.Send(new GetTerritoriesV1Query() { SessionId = (Guid)SessionId }).GetAwaiter().GetResult();
                    officeIds = new List<Guid>();
                    foreach (var shit in bla)
                    {
                        officeIds.Append((Guid)shit.Id);
                    }
                }

                return officeIds;
            }
        }
    }
}
