using System;
using System.Collections.Generic;
using MediatR;
using MySystem.SharedKernel.Entites.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Public.V1Dto;

namespace MySystem.Application.Employee.Query.GetEmployeeTerritories.V1
{
    public class GetEmployeeTerritoriesQuery : IRequest<ResponseDto<IEnumerable<EmployeeTerritoriesDto>>>
    {
        public Guid? EmployeeId { get; set; }

        public Guid? SessionId { get; set; }

    }
}
