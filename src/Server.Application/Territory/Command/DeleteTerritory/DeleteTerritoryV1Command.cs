using System;
using System.Collections.Generic;
using MediatR;

namespace MySystem.Application.Territory.Command.DeleteTerritory
{
    public class DeleteTerritoryV1Command : IRequest
    {
        public List<Guid> TerritoryIds { get; set; }
    }
}
