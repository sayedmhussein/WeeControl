using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel;

namespace WeeControl.WebApi.Services;

public class UserInfoService : ICurrentUserInfo
{
    public IEnumerable<Claim> Claims { get; }

    public Guid? SessionId { get; }
    
    public UserInfoService(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        Claims = httpContextAccessor?.HttpContext?.User.Claims;
        
        var idStr = Claims?.FirstOrDefault(c => c.Type == ClaimsValues.ClaimTypes.Session)?.Value;
        if (Guid.TryParse(idStr, out var idGuid))
        {
            SessionId = idGuid;
        }
        

        this.mediator = mediator;
    }

   
    
    
    
    
    
    
    
    private readonly IMediator mediator;
    private readonly ICollection<string> territories = new List<string>();
    
    public Guid? GetSessionId()
    {
        return SessionId;
    }

    public async Task<IEnumerable<string>> GetTerritoriesListAsync(CancellationToken cancellationToken)
    {
        if (territories.Count != 0) return territories;
            
        var territoryCode = Claims.FirstOrDefault(c => c.Type == ClaimsValues.ClaimTypes.Territory)?.Value;
        territories.Add(territoryCode);
        var cla = await mediator.Send(new TerritoryQuery(territoryCode), cancellationToken);

         foreach (var bra in cla.Payload)
         {
             territories.Add(bra.UniqueName);
         }

        return territories;
    }
}