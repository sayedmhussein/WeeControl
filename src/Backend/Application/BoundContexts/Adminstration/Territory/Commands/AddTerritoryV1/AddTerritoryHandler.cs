using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Common.Exceptions;
using WeeControl.Backend.Domain.Common.Interfaces;
using WeeControl.Backend.Domain.EntityGroups.Territory;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Territory;
using WeeControl.Common.SharedKernel.Extensions;

namespace WeeControl.Backend.Application.BoundContexts.Adminstration.Territory.Commands.AddTerritoryV1
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

                var existCount = context.Territories.Where(x => x.Name == dbo.Name && x.CountryId == dbo.CountryId).Count();
                if (existCount != 0)
                {
                    throw new ConflictFailureException();
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
                throw new NotImplementedException(e.Message);
            }
        }
    }
}
