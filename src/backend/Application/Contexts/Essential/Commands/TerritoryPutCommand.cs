// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using WeeControl.Application.Contexts.Essential.Queries;
// using WeeControl.Application.Exceptions;
// using WeeControl.Application.Interfaces;
// using WeeControl.Domain.Contexts.Essential;
// using WeeControl.SharedKernel.Essential.DataTransferObjects;
// using WeeControl.SharedKernel.RequestsResponses;
//
// namespace WeeControl.Application.Contexts.Essential.Commands;
//
// public class TerritoryPutCommand : IRequest
// {
//     private readonly RequestDto<TerritoryDto> dto;
//
//     public TerritoryPutCommand(RequestDto<TerritoryDto> dto)
//     {
//         this.dto = dto;
//     }
//     
//     public class AddOrEditTerritoryHandler : IRequestHandler<TerritoryPutCommand>
//     {
//         private readonly IEssentialDbContext essentialDbContext;
//         private readonly ICurrentUserInfo currentUserInfo;
//         private readonly IMediator mediator;
//
//         public AddOrEditTerritoryHandler(IEssentialDbContext essentialDbContext, ICurrentUserInfo currentUserInfo, IMediator mediator)
//         {
//             this.essentialDbContext = essentialDbContext;
//             this.currentUserInfo = currentUserInfo;
//             this.mediator = mediator;
//         }
//         
//         public async Task<Unit> Handle(TerritoryPutCommand request, CancellationToken cancellationToken)
//         {
//             var model = request.dto.Payload;
//             
//             if (string.IsNullOrWhiteSpace(model.CountryCode) || 
//                 string.IsNullOrWhiteSpace(model.TerritoryCode) ||
//                 string.IsNullOrWhiteSpace(model.TerritoryName))
//             {
//                 throw new BadRequestException("Territory code, name and country can't be empty!");
//             }
//
//             if (await mediator.Send(new TerritoryVerifyQuery(model.TerritoryCode), cancellationToken) == false)
//             {
//                 throw new NotAllowedException("You can't take this action using your account!");
//             }
//
//             if (essentialDbContext.Territories.FirstOrDefault(x => x.TerritoryIdObsolute == model.TerritoryCode) != null)
//             {
//                 throw new ConflictFailureException("Same territory exist!");
//             }
//             
//             await essentialDbContext.Territories.AddAsync(TerritoryDbo.Create(model), cancellationToken);
//             await essentialDbContext.SaveChangesAsync(cancellationToken);
//             
//             return Unit.Value;
//         }
//     }
// }