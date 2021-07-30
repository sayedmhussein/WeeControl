using System;
using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1;

namespace WeeControl.Server.Application.Territory.V1.Queries
{
    public class GetTerritoriesQuery : IRequest<IEnumerable<TerritoryDto>>
    {
        public Guid? TerritoryId { get; set; }

        public Guid? EmployeeId { get; set; }

        public Guid? SessionId { get; set; }
    }
}
