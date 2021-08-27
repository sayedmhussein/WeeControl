using System;
using System.Collections.Generic;
using MediatR;

namespace WeeControl.Backend.Application.Territory.Commands.DeleteTerritoriesV1
{
    public class DeleteTerritoryCommand : IRequest
    {
        public DeleteTerritoryCommand(Guid id)
        {
            TerritoryId = id;
        }

        public Guid TerritoryId { get; set; }
    }
}
