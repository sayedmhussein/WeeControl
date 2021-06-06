using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Application.Territory.Command.DeleteTerritories
{
    public class DeleteTerritoriesV1Handler : IRequestHandler<DeleteTerritoriesV1Command>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo userInfo;
        private readonly IValuesService values;

        public DeleteTerritoriesV1Handler(IMySystemDbContext context, ICurrentUserInfo userInfo, IValuesService values)
        {
            this.context = context ?? throw new ArgumentNullException("Db Context can't be Null!");
            this.userInfo = userInfo ?? throw new ArgumentNullException("User Info can't be Null!");
            this.values = values;
        }

        public Task<Unit> Handle(DeleteTerritoriesV1Command request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            if (request.TerritoryIds == null || request.TerritoryIds.Count == 0)
            {
                throw new BadRequestException("");
            }

            var tag = userInfo.Claims.FirstOrDefault(x => x.Type == values.ClaimType[ClaimTypeEnum.HumanResources])?.Value?.Contains(values.ClaimTag[ClaimTagEnum.Delete]);
            if (tag == false || tag == null)
            {
                throw new NotAllowedException("");
            }

            //Below bad performance in large
            try
            {
                foreach (var territoryid in request.TerritoryIds)
                {
                    var territory = context.Territories.FirstOrDefault(x => x.Id == territoryid);
                    if (territory == null)
                    {
                        throw new NotFoundException("", "");
                    }
                    context.Territories.RemoveRange(territory);
                    context.SaveChangesAsync(default).GetAwaiter().GetResult();
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch(Exception e)
            {
                _ = e.Message;
                throw;
                throw new DeleteFailureException("", "", "");
            }

            return Task.FromResult(Unit.Value);
        }
    }
}
