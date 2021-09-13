using System;
using MediatR;

namespace WeeControl.Backend.Application.EntityGroups.Territory.Commands.DeleteTerritoryV1
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
