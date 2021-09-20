using System.Collections.Generic;
using MediatR;
using WeeControl.Common.SharedKernel.DtosV1.Employee;

namespace WeeControl.Backend.Application.EntityGroups.Employee.Commands.UpdateEmployeeV1
{
    public class UpdateEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
    }
}
