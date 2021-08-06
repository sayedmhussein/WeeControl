using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.Aggregates.Employee.DtosV1;

namespace WeeControl.Server.Application.Aggregates.Employee.Commands.AddEmployeeV1
{
    public class AddEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
        public ICollection<EmployeeDto> List { get; set; }
    }
}
