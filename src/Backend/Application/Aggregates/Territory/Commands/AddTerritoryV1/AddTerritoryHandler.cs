using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Common.Exceptions;
using WeeControl.Backend.Domain.BasicDbos.Territory;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.SharedKernel.Aggregates.Territory.DtosV1;
using WeeControl.SharedKernel.Extensions;

namespace WeeControl.Backend.Application.Basic.Territory.Commands.AddTerritoryV1
{
    public class AddTerritoryHandler : IRequestHandler<AddTerritoryCommand, IdentifiedTerritoryDto>
    {
        private readonly IMySystemDbContext context;

        public AddTerritoryHandler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IdentifiedTerritoryDto> Handle(AddTerritoryCommand request, CancellationToken cancellationToken)
        {
            //if (request == null || request.TerritoryDtos == null)
            //{
            //    throw new ArgumentNullException();
            //}

            try
            {
                var dbo = request.Payload.ToDbo<TerritoryDto, TerritoryDbo>();
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

                return dbo.ToDto<TerritoryDbo, IdentifiedTerritoryDto>();
            }
            catch (ConflictFailureException)
            {
                throw;
            }
            catch (DbUpdateException)
            {
                throw new ConflictFailureException();
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
        }
    }
}
