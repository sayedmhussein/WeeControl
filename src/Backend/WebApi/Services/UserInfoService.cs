using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetListOfTerritories;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Common.UserSecurityLib.Enums;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.WebApi.Services
{
    public class UserInfoService : ICurrentUserInfo
    {
        private readonly IMediator mediatR;
        private readonly IUserClaimService employeeAttribute;
        private Guid? sessionid = null;
        private readonly ICollection<string> territories = new List<string>();
        
        public UserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediatR, IUserClaimService values)
        {
            Claims = httpContextAccessor?.HttpContext?.User.Claims;

            this.mediatR = mediatR;
            employeeAttribute = values;
        }

        public IEnumerable<Claim> Claims { get; private set; }

        public Guid? GetSessionId()
        {
            if (sessionid != null) return sessionid;
            
            var sessionid_ = Claims.FirstOrDefault(c => c.Type == employeeAttribute.GetClaimType(ClaimTypeEnum.Session))?.Value;
            if (Guid.TryParse(sessionid_, out Guid sessionId__))
            {
                sessionid = sessionId__;
            }

            return sessionid;
        }

        public IEnumerable<Claim> GetClaimList()
        {
            return Claims;
        }

        public async Task<IEnumerable<string>> GetTerritoriesListAsync()
        {
            if (territories.Count != 0) return territories;
            
            var territoryid_ = Claims.FirstOrDefault(c => c.Type == employeeAttribute.GetClaimType(ClaimTypeEnum.Territory))?.Value;
            territories.Add(territoryid_);
            var cla = await mediatR.Send(new GetTerritoriesQuery(territoryid_));

            // foreach (var bra in cla.Payload)
            // {
            //     territories.Add(bra.Id.ToString());
            // }

            return territories;
        }
    }
}