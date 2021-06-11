using System;
using System.Collections.Generic;
using MediatR;
using MySystem.SharedKernel.EntityV1Dtos.Territory;

namespace MySystem.Application.Territory.Query.GetTerritories
{
    public class GetTerritoriesV1Query : IRequest<IEnumerable<TerritoryDto>>
    {
        public Guid? TerritoryId { get; set; }

        public Guid? EmployeeId { get; set; }

        public Guid? SessionId { get; set; }
    }
}
