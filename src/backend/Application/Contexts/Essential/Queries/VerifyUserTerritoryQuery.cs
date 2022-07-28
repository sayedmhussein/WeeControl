using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts.Essential;

namespace WeeControl.Application.Contexts.Essential.Queries;

public class VerifyUserTerritoryQuery : IRequest<bool>
{
    private readonly string territoryCode;

    public VerifyUserTerritoryQuery(string territoryCode)
    {
        this.territoryCode = territoryCode;
    }

    public class VerifyUserTerritoryHandler : IRequestHandler<VerifyUserTerritoryQuery, bool>
    {
        private readonly IEssentialDbContext essentialDbContext;
        private readonly ICurrentUserInfo currentUser;

        public VerifyUserTerritoryHandler(IEssentialDbContext essentialDbContext, ICurrentUserInfo currentUser)
        {
            this.essentialDbContext = essentialDbContext;
            this.currentUser = currentUser;
        }
        
        public async Task<bool> Handle(VerifyUserTerritoryQuery request, CancellationToken cancellationToken)
        {
            var mainTerritory = await essentialDbContext.Sessions
                .Include(x => x.User).Where(x => x.SessionId == currentUser.GetSessionId())
                .Select(x => x.User.TerritoryId).FirstOrDefaultAsync(cancellationToken);

            if (mainTerritory is null)
            {
                return false;
            }

            var ids = new List<string>() {};
            await essentialDbContext.Territories.Where(x => ids.Contains(x.ReportToId)).ForEachAsync(x =>
            {
                ids.Add(x.TerritoryId);
            }, cancellationToken);

            return ids.Contains(request.territoryCode);
        }
    }
}