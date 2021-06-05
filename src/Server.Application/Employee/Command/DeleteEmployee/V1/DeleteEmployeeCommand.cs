using System;
using MediatR;

namespace Application.Employee.Command.DeleteEmployee.V1
{
    public class DeleteEmployeeCommand : IRequest
    {
        public string Id { get; set; }
    }
}
