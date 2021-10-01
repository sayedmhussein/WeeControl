using System;
using MediatR;

namespace WeeControl.Application.HumanResources.Commands.DeleteTerritoryV1
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
