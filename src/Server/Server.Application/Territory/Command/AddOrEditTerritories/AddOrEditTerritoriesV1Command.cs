using System.Collections.Generic;
using MediatR;
using MySystem.SharedKernel.EntityV1Dtos.Territory;

namespace MySystem.Application.Territory.Command.AddOrEditTerritories
{
    public class AddOrEditTerritoriesV1Command : IRequest<IEnumerable<TerritoryDto>>
    {
        public ICollection<TerritoryDto> TerritoryDtos { get; set; }
    }
}
