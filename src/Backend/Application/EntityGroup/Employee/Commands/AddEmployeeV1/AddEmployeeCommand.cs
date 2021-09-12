using System.Collections.Generic;
using MediatR;
using WeeControl.SharedKernel.EntityGroup.Employee.DtosV1;

namespace WeeControl.Backend.Application.EntityGroup.Employee.Commands.AddEmployeeV1
{
    public class AddEmployeeCommand : IRequest<IEnumerable<EmployeeDto>>
    {
        public ICollection<EmployeeDto> List { get; set; }
    }
}
