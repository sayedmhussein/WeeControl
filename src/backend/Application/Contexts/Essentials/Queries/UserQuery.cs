// using System;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using WeeControl.Core.Application.Interfaces;
// using WeeControl.Core.DataTransferObject.BodyObjects;
// using WeeControl.Core.Domain.Contexts.Essentials;
// using WeeControl.Core.Domain.Interfaces;
//
// namespace WeeControl.Core.Application.Contexts.Essentials.Queries;
//
// public class UserQuery : IRequest<ResponseDto<IEnumerable<UserDtoV1>>>
// {
//     private readonly string username;
//     private readonly string territory;
//     private readonly bool detailed;
//     private readonly bool includeActivity;
//     private readonly DateTime? includeUntil;
//
//     public UserQuery(string username = null, string territory = null, bool detailed = false, bool includeActivity = false, DateTime? includeUntil = null)
//     {
//         this.username = username;
//         this.territory = territory;
//         this.detailed = detailed;
//         this.includeActivity = includeActivity;
//         this.includeUntil = includeUntil;
//     }
//     
//     public class UserHandler : IRequestHandler<UserQuery, ResponseDto<IEnumerable<UserDtoV1>>>
//     {
//         private readonly IEssentialDbContext context;
//         private readonly ICurrentUserInfo userInfo;
//         private readonly IMediator mediator;
//
//         public UserHandler(IEssentialDbContext context, ICurrentUserInfo userInfo, IMediator mediator)
//         {
//             this.context = context;
//             this.userInfo = userInfo;
//             this.mediator = mediator;
//         }
//     
//         public async Task<ResponseDto<IEnumerable<UserDtoV1>>> Handle(UserQuery request, CancellationToken cancellationToken)
//         {
//             var listOfUsersDbo = new List<UserDbo>();
//             var listOfUsersDto = new List<UserDtoV1>();
//
//             if (request.territory is null && request.username is null)
//             {
//                 var user = await context.UserSessions
//                     .Include(x => x.User)
//                     .FirstOrDefaultAsync(x => x.SessionId == userInfo.SessionId, cancellationToken);
//                 
//                 listOfUsersDbo.Add(user?.User?? throw new NullReferenceException());
//             }
//
//             // var dfd = await mediator.Send(new TerritoryQuery(userInfo.Claims.First(c => c.Type == ClaimsValues.ClaimTypes.Territory).Value), cancellationToken);
//             // var dff = dfd.Payload.Select(x => x.TerritoryCode);
//             
//             var allowed = await userInfo.GetTerritoriesListAsync(cancellationToken);
//             foreach (var userDbo in await context.Users.Where(x => allowed.Contains(x.TerritoryId)).ToListAsync(cancellationToken))
//             {
//                 listOfUsersDto.Add(new UserDtoV1()
//                 {
//                     Email = userDbo.Email, Username = userDbo.Username, TerritoryCode = userDbo.TerritoryId
//                 });
//             }
//
//             var response = ResponseDto.Create<IEnumerable<UserDtoV1>>(listOfUsersDto);//<IEnumerable<UserDtoV1>>(users);
//             //var response = new ResponseDto<IEnumerable<UserDtoV1>>(users);
//             return response;
//         }
//     }
// }

