using System.Collections.Generic;
using MediatR;
using MySystem.SharedKernel.EntityV1Dtos.Employee;

namespace Application.Employee.Command.AddEmployeeClaim.V1
{
    public class AddEmployeeClaimCommand : IRequest<IEnumerable<EmployeeClaimDto>>
    {
    }
}
