using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.EntityGroup.Employee.DtosV1;

namespace WeeControl.Backend.Application.EntityGroup.Employee.Commands.AssignClaimV1
{
    public class AddEmployeeClaimCommand : IRequest<IEnumerable<EmployeeClaimDto>>
    {
    }
}
