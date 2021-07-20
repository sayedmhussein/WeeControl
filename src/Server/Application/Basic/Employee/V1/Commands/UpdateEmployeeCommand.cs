using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.BasicSchemas.Employee.Entities.DtosV1;

namespace WeeControl.Server.Application.Employee.Command.UpdateEmployee.V1
{
    public class UpdateEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
    }
}
