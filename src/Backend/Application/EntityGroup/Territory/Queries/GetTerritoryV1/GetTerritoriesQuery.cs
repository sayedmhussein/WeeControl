using System;
using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.EntityGroup.Territory.DtosV1;

namespace WeeControl.Backend.Application.Territory.Queries.GetTerritoryV1
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
