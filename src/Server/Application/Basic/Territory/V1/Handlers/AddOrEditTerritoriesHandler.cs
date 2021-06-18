using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Application.Territory.V1.Commands;
using WeeControl.Server.Domain.BasicDbos.Territory;
using WeeControl.Server.Domain.Extensions;
using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;

namespace WeeControl.Server.Application.Territory.V1.Handlers
{
    public class AddOrEditTerritoriesHandler : IRequestHandler<AddOrEditTerritoriesCommand, IEnumerable<TerritoryDto>>
    {
        private readonly IMySystemDbContext context;

        public AddOrEditTerritoriesHandler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TerritoryDto>> Handle(AddOrEditTerritoriesCommand request, CancellationToken cancellationToken)
        {
            var responses = new List<TerritoryDto>();

            if (request.TerritoryDtos is null || request.TerritoryDtos.Count == 0)
            {
                throw new BadRequestException("");
            }
            foreach (var dto in request.TerritoryDtos)
            {
                try
                {
                    var dbo = dto.ToDbo<TerritoryDto, TerritoryDbo>();

                    if (dto.Id == null || dto.Id == Guid.Empty)
                    {
                        await context.Territories.AddAsync(dbo, cancellationToken);
                    }
                    else
                    {
                        context.Territories.Update(dbo);
                    }

                    await context.SaveChangesAsync(cancellationToken);

                    responses.Add(dbo.ToDto<TerritoryDbo, TerritoryDto>());
                }
                catch
                {
                    //Log Please...
                }
            }

            return responses;
        }
    }
}
