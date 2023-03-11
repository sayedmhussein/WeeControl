// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using WeeControl.Core.DataTransferObject.BodyObjects;
// using WeeControl.Core.DataTransferObject.Contexts.Temporary.DataTransferObjects;
// using WeeControl.Core.Domain.Interfaces;
//
// namespace WeeControl.Core.Application.Contexts.Developer;
//
// public class TerritoryQuery : IRequest<ResponseDto<IEnumerable<TerritoryDto>>>
// {
//     private readonly ICollection<string> territoryNames;
//
//     public TerritoryQuery()
//     {
//         territoryNames = null;
//     }
//
//     public TerritoryQuery(string territoryName)
//     {
//         territoryNames = new List<string>() { territoryName };
//     }
//
//     public TerritoryQuery(ICollection<string> territoryNames)
//     {
//         this.territoryNames = territoryNames;
//     }
//
//     public class GetListOfTerritoriesHandler : IRequestHandler<TerritoryQuery, ResponseDto<IEnumerable<TerritoryDto>>>
//     {
//         private readonly IEssentialDbContext context;
//
//         public GetListOfTerritoriesHandler(IEssentialDbContext context)
//         {
//             this.context = context;
//         }
//
//         public async Task<ResponseDto<IEnumerable<TerritoryDto>>> Handle(TerritoryQuery request, CancellationToken cancellationToken)
//         {
//             var list = new List<TerritoryDto>();
//
//             if (request.territoryNames is null)
//             {
//                 await context.Territories.ForEachAsync(x =>
//                 {
//                     list.Add(new TerritoryDto()
//                     {
//                         AlternativeName = x.AlternativeName,
//                         UniqueName = x.UniqueName,
//                         CountryCode = x.CountryCode
//                     });
//                 }, cancellationToken);
//             }
//             else
//             {
//                 var ids = new List<string>();
//                 ids.AddRange(request.territoryNames);
//
//                 await context.Territories.Where(x => ids.Contains(x.UniqueName)).ForEachAsync(x =>
//                 {
//                     ids.Add(x.UniqueName);
//                 }, cancellationToken);
//
//                 var q = context.Territories.Include(a => a.ReportingTo).Where(x => ids.Contains(x.UniqueName));
//
//                 await q.ForEachAsync(x =>
//                 {
//                     list.Add(new TerritoryDto()
//                     {
//                         AlternativeName = x.AlternativeName,
//                         UniqueName = x.UniqueName,
//                         CountryCode = x.CountryCode
//                     });
//                 }, cancellationToken);
//             }
//
//             return ResponseDto.Create<IEnumerable<TerritoryDto>>(list);
//         }
//     }
// }

