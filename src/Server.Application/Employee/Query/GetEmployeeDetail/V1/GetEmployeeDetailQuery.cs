using System;
using MediatR;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Query.GetEmployeeDetail.V1
{
    public class GetEmployeeDetailQuery : IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }
    }
}
