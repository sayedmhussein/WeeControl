using System;
using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1;

namespace WeeControl.Server.Application.Territory.Queries.GetTerritoryV1
{
    public class GetTerritoriesQuery : IRequest<IEnumerable<TerritoryWithIdDto>>
    {
        public GetTerritoriesQuery(Guid? id)
        {
            TerritoryId = id;
        }

        public Guid? TerritoryId { get; set; }
    }
}
