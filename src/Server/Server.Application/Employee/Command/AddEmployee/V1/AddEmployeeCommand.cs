using System.Collections.Generic;
using MediatR;
using MySystem.SharedKernel.EntityV1Dtos.Employee;

namespace MySystem.Application.Employee.Command.AddEmployee.V1
{
    public class AddEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
        public ICollection<EmployeeDto> List { get; set; }
    }
}
