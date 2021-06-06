using System.Collections.Generic;
using MediatR;
using MySystem.SharedKernel.EntityV1Dtos.Employee;

namespace Application.Employee.Command.UpdateEmployee.V1
{
    public class UpdateEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
    }
}
