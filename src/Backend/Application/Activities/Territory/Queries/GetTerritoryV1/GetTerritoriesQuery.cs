using System;
using System.Collections.Generic;
using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Territory;

namespace WeeControl.Backend.Application.Activities.Territory.Queries.GetTerritoryV1
{
    public class GetTerritoriesQuery : IRequest<IEnumerable<IdentifiedTerritoryDto>>
    {
        public GetTerritoriesQuery(Guid? id)
        {
            TerritoryId = id;
        }

        public Guid? TerritoryId { get; set; }
    }
}
