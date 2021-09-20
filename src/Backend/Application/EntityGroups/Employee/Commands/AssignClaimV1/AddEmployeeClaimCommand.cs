using System.Collections.Generic;
using MediatR;
using WeeControl.Common.SharedKernel.DtosV1.Employee;

namespace WeeControl.Backend.Application.EntityGroups.Employee.Commands.AssignClaimV1
{
    public class AddEmployeeClaimCommand : IRequest<IEnumerable<EmployeeClaimDto>>
    {
    }
}
