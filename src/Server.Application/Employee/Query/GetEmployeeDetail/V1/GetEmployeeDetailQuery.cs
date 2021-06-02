using System;
using MediatR;
using MySystem.SharedKernel.Entities.Employee.V1Dto;

namespace Application.Employee.Query.GetEmployeeDetail.V1
{
    public class GetEmployeeDetailQuery : IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }
    }
}
