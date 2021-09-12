using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.EntityGroup.Employee.DtosV1;

namespace WeeControl.Backend.Application.EntityGroup.Employee.Commands.UpdateEmmployeeV1
{
    public class UpdateEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
    }
}
