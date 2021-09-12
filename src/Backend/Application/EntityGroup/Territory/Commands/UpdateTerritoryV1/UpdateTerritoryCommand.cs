using System;
using MediatR;
using WeeControl.SharedKernel.EntityGroup.Territory.DtosV1;

namespace WeeControl.Backend.Application.Territory.Commands.UpdateTerritoryV1
{
    public class UpdateTerritoryCommand : IRequest<Unit>
    {
        public UpdateTerritoryCommand(Guid id, TerritoryDto dto)
        {
            Id = id;
            TerritoryDto = dto;
        }

        public Guid Id { get; set; }
        public TerritoryDto TerritoryDto { get; set; }
    }
}
