using System;
using MediatR;
using MySystem.SharedKernel.EntityV1Dtos.Employee;

namespace Application.Employee.Query.GetEmployeeDetail.V1
{
    public class GetEmployeeDetailQuery : IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }
    }
}
