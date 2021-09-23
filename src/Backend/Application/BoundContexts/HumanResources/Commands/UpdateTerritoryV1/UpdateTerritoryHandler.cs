﻿// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using WeeControl.Backend.Application.Exceptions;
// using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
//
// namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.UpdateTerritoryV1
// {
//     public class UpdateTerritoryHandler : IRequestHandler<UpdateTerritoryCommand, Unit>
//     {
//         private readonly IHumanResourcesDbContext context;
//
//         public UpdateTerritoryHandler(IHumanResourcesDbContext context)
//         {
//             this.context = context;
//         }
//
//         public async Task<Unit> Handle(UpdateTerritoryCommand request, CancellationToken cancellationToken)
//         {
//             if (request == null || request.TerritoryDto == null)
//             {
//                 throw new ArgumentNullException();
//             }
//
//             //var dbo = request.TerritoryDto.ToDbo<TerritoryDto, TerritoryDbo>();
//
//             //var item = await context.Territories.FirstOrDefaultAsync(x => x.Id == request.TerritoryDto.Id);
//             var item = await context.Territories.FirstOrDefaultAsync(x => x.Id == request.Id);
//             if (item == null)
//             {
//                 throw new NotFoundException();
//             }
//
//             
//
//             try
//             {
//                 item.CountryId = request.TerritoryDto.CountryId;
//                 item.Name = request.TerritoryDto.Name;
//                 item.ReportToId = request.TerritoryDto.ReportToId;
//
//                 context.Territories.Update(item);
//                 await context.SaveChangesAsync(cancellationToken);
//             }
//             catch (Exception ex)
//             {
//                 _ = ex.Message;
//                 throw;
//             }
//
//             
//
//             return Unit.Value;
//         }
//     }
// }
