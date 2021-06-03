using System;
using System.Collections.Generic;
using MediatR;

namespace MySystem.Application.Employee.Query.GetEmployeeTerritories
{
    public class GetEmployeeTerritoriesQuery : IRequest<IEnumerable<Guid>>
    {
        public Guid SessionId { get; set; }
    }
}
