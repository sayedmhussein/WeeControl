﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using WeeControl.Backend.Application.Common.Interfaces;
using WeeControl.Backend.Application.Territory.Queries.GetTerritoryV1;
using WeeControl.SharedKernel.Aggregates.Employee;
using WeeControl.SharedKernel.Aggregates.Employee.Enums;

namespace WeeControl.Backend.WebApi.Services
{
    public class UserInfoService : ICurrentUserInfo
    {
        private readonly IMediator mediatR;
        private readonly ICollection<Guid> officeIds = new List<Guid>();
        
        public UserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediatR, IEmployeeLists values)
        {
            Claims = httpContextAccessor.HttpContext.User.Claims;

            var sessionid_ = Claims.FirstOrDefault(c => c.Type == values.GetClaimType(ClaimTypeEnum.Session))?.Value;
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
                    //var cla = mediatR.Send(new GetTerritoriesQuery() { SessionId = (Guid)SessionId });
                    var cla = mediatR.Send(new GetTerritoriesQuery(SessionId));
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