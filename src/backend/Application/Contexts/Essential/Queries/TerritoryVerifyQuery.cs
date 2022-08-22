// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using WeeControl.Application.Interfaces;
//
// namespace WeeControl.Application.Contexts.Essential.Queries;
//
// public class TerritoryVerifyQuery : IRequest<bool>
// {
//     private readonly string territoryCode;
//
//     public TerritoryVerifyQuery(string territoryCode)
//     {
//         this.territoryCode = territoryCode;
//     }
//
//     public class VerifyUserTerritoryHandler : IRequestHandler<TerritoryVerifyQuery, bool>
//     {
//         private readonly IEssentialDbContext essentialDbContext;
//         private readonly ICurrentUserInfo currentUser;
//         private readonly IMediator mediator;
//
//         public VerifyUserTerritoryHandler(IEssentialDbContext essentialDbContext, ICurrentUserInfo currentUser, IMediator mediator)
//         {
//             this.essentialDbContext = essentialDbContext;
//             this.currentUser = currentUser;
//             this.mediator = mediator;
//         }
//         
//         public async Task<bool> Handle(TerritoryVerifyQuery request, CancellationToken cancellationToken)
//         {
//             var userTerritory = await essentialDbContext.UserSessions
//                 .Include(x => x.User)
//                 .Where(x => x.SessionId == currentUser.SessionId)
//                 .Select(x => x.User.TerritoryId).FirstOrDefaultAsync(cancellationToken);
//
//             if (userTerritory is null)
//             {
//                 return false;
//             }
//
//             var list = await mediator.Send(new TerritoryQuery(userTerritory), cancellationToken);
//
//             return list.Payload.Select(x => x.TerritoryCode).Contains(request.territoryCode);
//         }
//     }
// }