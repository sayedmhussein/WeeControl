using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;

namespace WeeControl.Server.Application.Aggregates.Employee.Commands.UpdateEmmployeeV1
{
    public class UpdateEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
    }
}
