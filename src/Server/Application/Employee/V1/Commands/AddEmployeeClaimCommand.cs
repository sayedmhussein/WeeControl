using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.CommonSchemas.Employee.DtosV1;

namespace WeeControl.Server.Application.Employee.Command.AddEmployeeClaim.V1
{
    public class AddEmployeeClaimCommand : IRequest<IEnumerable<EmployeeClaimDto>>
    {
    }
}
