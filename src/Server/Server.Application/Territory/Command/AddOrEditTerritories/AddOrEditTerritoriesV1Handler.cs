using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo.Territory;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.EntityV1Dtos.Territory;

namespace MySystem.Application.Territory.Command.AddOrEditTerritories
{
    public class AddOrEditTerritoriesV1Handler : IRequestHandler<AddOrEditTerritoriesV1Command, IEnumerable<TerritoryDto>>
    {
        private readonly IMySystemDbContext context;

        public AddOrEditTerritoriesV1Handler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TerritoryDto>> Handle(AddOrEditTerritoriesV1Command request, CancellationToken cancellationToken)
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
