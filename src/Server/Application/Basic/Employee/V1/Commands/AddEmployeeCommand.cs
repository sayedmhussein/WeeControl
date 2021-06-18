using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1;

namespace WeeControl.Server.Application.Employee.Command.AddEmployee.V1
{
    public class AddEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
        public ICollection<EmployeeDto> List { get; set; }
    }
}
