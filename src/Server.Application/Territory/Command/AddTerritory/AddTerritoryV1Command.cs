using System.Collections.Generic;
using MediatR;
using MySystem.SharedKernel.Entities.Territory.V1Dto;

namespace MySystem.Application.Territory.Command.AddTerritory
{
    public class AddTerritoryV1Command : IRequest<IEnumerable<TerritoryDto>>
    {
        public ICollection<TerritoryDto> TerritoryDtos { get; set; }
    }
}
