using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Application.Basic.Territory.V1.Commands;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Domain.BasicDbos.Territory;
using WeeControl.Server.Domain.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Territory.Entities.DtosV1;
using WeeControl.SharedKernel.Extensions;

namespace WeeControl.Server.Application.Basic.Territory.V1.Handlers
{
    public class AddTerritoryHandler : IRequestHandler<AddTerritoryCommand, Unit>
    {
        private readonly IMySystemDbContext context;

        public AddTerritoryHandler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(AddTerritoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.TerritoryDto == null)
            {
                throw new ArgumentNullException();
            }

            var dbo = request.TerritoryDto.ToDbo<TerritoryDto, TerritoryDbo>();

            if (dbo.ReportToId != null)
            {
                var territory = await context.Territories.FirstOrDefaultAsync(x => x.Id == dbo.ReportToId, cancellationToken);
                if (territory == null)
                {
                    throw new ConflictFailureException();
                }
            }

            await context.Territories.AddAsync(dbo, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
