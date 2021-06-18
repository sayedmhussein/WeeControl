using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Application.Territory.V1.Commands;
using WeeControl.SharedKernel.CommonSchemas.Employee.Dicts;
using WeeControl.SharedKernel.CommonSchemas.Employee.Enums;
using WeeControl.SharedKernel.CommonSchemas.Territory.Interfaces;

namespace WeeControl.Server.Application.Territory.Commands.DeleteTerritories
{
    public class DeleteTerritoriesHandler : IRequestHandler<DeleteTerritoriesCommand>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo userInfo;
        private readonly ITerritoryDicts values;
        private readonly IClaimDicts employeeValues;

        public DeleteTerritoriesHandler(IMySystemDbContext context, ICurrentUserInfo userInfo, ITerritoryDicts values, IClaimDicts employeeValues)
        {
            this.context = context ?? throw new ArgumentNullException("Db Context can't be Null!");
            this.userInfo = userInfo ?? throw new ArgumentNullException("User Info can't be Null!");
            this.values = values;
            this.employeeValues = employeeValues;
        }

        public Task<Unit> Handle(DeleteTerritoriesCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            if (request.TerritoryIds == null || request.TerritoryIds.Count == 0)
            {
                throw new BadRequestException("");
            }

            var tag = userInfo.Claims.FirstOrDefault(x => x.Type == employeeValues.ClaimType[ClaimTypeEnum.HumanResources])?.Value?.Contains(employeeValues.ClaimTag[ClaimTagEnum.Delete]);
            if (tag == false || tag == null)
            {
                throw new NotAllowedException("");
            }

            //todo: refactor as below is very bad performance!
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
