using System;
using System.Collections.Generic;
using MediatR;

namespace MySystem.Application.Territory.Command.DeleteTerritories
{
    public class DeleteTerritoriesV1Command : IRequest
    {
        public List<Guid> TerritoryIds { get; set; }
    }
}
