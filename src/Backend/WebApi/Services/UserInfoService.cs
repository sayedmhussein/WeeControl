using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using WeeControl.Backend.Application.Common.Interfaces;
using WeeControl.Backend.Application.Territory.Queries.GetTerritoryV1;
using WeeControl.SharedKernel.EntityGroup.Employee;
using WeeControl.SharedKernel.EntityGroup.Employee.Enums;

namespace WeeControl.Backend.WebApi.Services
{
    public class UserInfoService : ICurrentUserInfo
    {
        private readonly IMediator mediatR;
        private readonly IEmployeeLists employeeLists;
        private Guid? sessionid = null;
        private readonly ICollection<Guid> territories = new List<Guid>();
        
        public UserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediatR, IEmployeeLists values)
        {
            Claims = httpContextAccessor.HttpContext.User.Claims;

            this.mediatR = mediatR;
            employeeLists = values;
        }

        public IEnumerable<Claim> Claims { get; private set; }

        public Guid? SessionId
        {
            get
            {
                if (sessionid == null)
                {
                    var sessionid_ = Claims.FirstOrDefault(c => c.Type == employeeLists.GetClaimType(ClaimTypeEnum.Session))?.Value;
                    if (Guid.TryParse(sessionid_, out Guid sessionId__))
                    {
                        sessionid = sessionId__;
                    }
                }

                return sessionid;
            }
        }

        public IEnumerable<Guid> Territories
        {
            get
            {
                if (territories.Count == 0)
                {
                    var territoryid_ = Claims.FirstOrDefault(c => c.Type == employeeLists.GetClaimType(ClaimTypeEnum.Territory))?.Value;
                    if (Guid.TryParse(territoryid_, out Guid territoryid__))
                    {
                        territories.Add(territoryid__);
                        var cla = mediatR.Send(new GetTerritoriesQuery(territoryid__));
                        var bla = cla.GetAwaiter().GetResult();

                        foreach (var bra in bla)
                        {
                            territories.Add(bra.Id);
                        }
                    }
                }

                return territories;
            }
        }
    }
}