using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using WeeControl.Application.Contexts.Essential.Commands;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel;

namespace WeeControl.WebApi.Services;

public class UserInfoService : ICurrentUserInfo
{
    private readonly IMediator mediator;
    private Guid? sessionid = null;
    private readonly ICollection<string> territories = new List<string>();
        
    public UserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        Claims = httpContextAccessor?.HttpContext?.User.Claims;

        this.mediator = mediator;
    }

    public IEnumerable<Claim> Claims { get; private set; }

    public Guid? GetSessionId()
    {
        if (sessionid != null) return sessionid;

        var sessionGuid = Claims?.FirstOrDefault(c => c.Type == ClaimsTagsList.Claims.Session)?.Value;
        if (Guid.TryParse(sessionGuid, out Guid sessionString))
        {
            sessionid = sessionString;
        }

        return sessionid;
    }

    public IEnumerable<Claim> GetClaimList()
    {
        return Claims;
    }

    public async Task<IEnumerable<string>> GetTerritoriesListAsync(CancellationToken cancellationToken)
    {
        if (territories.Count != 0) return territories;
            
        var territoryCode = Claims.FirstOrDefault(c => c.Type == ClaimsTagsList.Claims.Territory)?.Value;
        territories.Add(territoryCode);
        var cla = await mediator.Send(new GetListOfTerritoriesQuery(territoryCode), cancellationToken);

         foreach (var bra in cla.Payload)
         {
             territories.Add(bra.TerritoryCode);
         }

        return territories;
    }

    public Task LogUserActivityAsync(string context, string details, CancellationToken cancellationToken)
    {
        return mediator.Send(new LogActivityCommand(context, details, GetSessionId()), cancellationToken);
    }
}