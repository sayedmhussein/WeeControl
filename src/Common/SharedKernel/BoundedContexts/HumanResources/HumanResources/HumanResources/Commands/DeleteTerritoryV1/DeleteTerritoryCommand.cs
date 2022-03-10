using System;
using MediatR;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.DeleteTerritoryV1
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
