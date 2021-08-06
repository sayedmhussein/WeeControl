using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;

namespace WeeControl.Server.Application.Aggregates.Employee.Commands.AssignClaimV1
{
    public class AddEmployeeClaimCommand : IRequest<IEnumerable<EmployeeClaimDto>>
    {
    }
}
