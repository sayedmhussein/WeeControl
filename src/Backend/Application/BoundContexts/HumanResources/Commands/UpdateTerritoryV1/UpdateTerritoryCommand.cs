using System;
using MediatR;
using WeeControl.Common.SharedKernel.Obsolute.Territory;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.UpdateTerritoryV1
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
