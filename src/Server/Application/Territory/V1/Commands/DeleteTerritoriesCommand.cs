using System;
using System.Collections.Generic;
using MediatR;

namespace WeeControl.Server.Application.Territory.V1.Commands
{
    public class DeleteTerritoriesCommand : IRequest
    {
        public List<Guid> TerritoryIds { get; set; }
    }
}
