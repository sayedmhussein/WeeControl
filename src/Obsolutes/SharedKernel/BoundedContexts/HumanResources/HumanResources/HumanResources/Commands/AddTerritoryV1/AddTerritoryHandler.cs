// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using WeeControl.Backend.Application.Exceptions;
// using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
// using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities;
// using WeeControl.Common.SharedKernel.DataTransferObjectV1.Territory;
// using WeeControl.Common.SharedKernel.Extensions;
//
// namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.AddTerritoryV1
// {
//     public class AddTerritoryHandler : IRequestHandler<AddTerritoryCommand, IdentifiedTerritoryDto>
//     {
//         private readonly IHumanResourcesDbContext context;
//
//         public AddTerritoryHandler(IHumanResourcesDbContext context)
//         {
//             this.context = context;
//         }
//
//         public async Task<IdentifiedTerritoryDto> Handle(AddTerritoryCommand request, CancellationToken cancellationToken)
//         {
//             //if (request == null || request.TerritoryDtos == null)
//             //{
//             //    throw new ArgumentNullException();
//             //}
//
//             try
//             {
//                 var dbo = request.Payload.ToDbo<TerritoryDto, Territory>();
//                 if (dbo.ReportToId != null)
//                 {
//                     var territory = await context.Territories.FirstOrDefaultAsync(x => x.Id == dbo.ReportToId, cancellationToken);
//                     if (territory == null)
//                     {
//                         throw new ConflictFailureException();
//                     }
//                 }
//
//                 var existCount = context.Territories.Where(x => x.Name == dbo.Name && x.CountryId == dbo.CountryId).Count();
//                 if (existCount != 0)
//                 {
//                     throw new ConflictFailureException();
//                 }
//
//                 await context.Territories.AddAsync(dbo, cancellationToken);
//                 await context.SaveChangesAsync(cancellationToken);
//
//                 return dbo.ToDto<Territory, IdentifiedTerritoryDto>();
//             }
//             catch (ConflictFailureException)
//             {
//                 throw;
//             }
//             catch (DbUpdateException)
//             {
//                 throw new ConflictFailureException();
//             }
//             catch (Exception e)
//             {
//                 throw new NotImplementedException(e.Message);
//             }
//         }
//     }
// }
