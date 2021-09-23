using System.Collections.Generic;
using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;

namespace WeeControl.Backend.Application.BoundContexts.Garbag.AssignClaimV1
{
    public class AddEmployeeClaimCommand : IRequest<IEnumerable<EmployeeClaimDto>>
    {
    }
}
