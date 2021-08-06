using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Domain.BasicDbos.Territory;
using WeeControl.Server.Domain.Interfaces;
using WeeControl.SharedKernel.Aggregates.Territory.DtosV1;
using WeeControl.SharedKernel.Extensions;

namespace WeeControl.Server.Application.Basic.Territory.Commands.AddTerritoryV1
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
            if (request == null || request.TerritoryDtos == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                foreach (var dto in request.TerritoryDtos)
                {
                    var dbo = dto.ToDbo<TerritoryDto, TerritoryDbo>();
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
                }
            }
            catch (ConflictFailureException)
            {
                throw;
            }
            catch
            {
                throw new NotImplementedException();
            }

            return Unit.Value;
        }
    }
}
