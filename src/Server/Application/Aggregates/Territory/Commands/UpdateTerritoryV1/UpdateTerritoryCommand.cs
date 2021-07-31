using System;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1;

namespace WeeControl.Server.Application.Territory.Commands.UpdateTerritoryV1
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
