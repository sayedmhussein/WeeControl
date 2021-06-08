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

namespace MySystem.Api.Services
{
    public class UserInfoService : ICurrentUserInfo
    {
        private readonly IMediator mediatR;
        private readonly ICollection<Guid> officeIds = new List<Guid>();
        
        public UserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediatR, ISharedValues values)
        {
            Claims = httpContextAccessor.HttpContext.User.Claims;

            var sessionid_ = Claims.FirstOrDefault(c => c.Type == values.ClaimType[ClaimTypeEnum.Session])?.Value;
            _ = Guid.TryParse(sessionid_, out Guid sessionId__);
            SessionId = sessionId__;

            this.mediatR = mediatR;
        }

        public Guid? SessionId { get; private set; }

        public IEnumerable<Claim> Claims { get; private set; }

        public IEnumerable<Guid> TerritoriesId
        {
            get
            {
                if (officeIds.Any() == false)
                {
                    var cla = mediatR.Send(new GetTerritoriesV1Query() { SessionId = (Guid)SessionId });
                    var bla = cla.GetAwaiter().GetResult();

                    foreach (var bra in bla)
                    {
                        officeIds.Add((Guid)bra.Id);
                    }
                }

                _ = officeIds;

                return officeIds;
            }
        }
    }
}