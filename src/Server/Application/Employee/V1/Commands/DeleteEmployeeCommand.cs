using System;
using MediatR;

namespace WeeControl.Server.Application.Employee.Command.DeleteEmployee.V1
{
    public class DeleteEmployeeCommand : IRequest
    {
        public string Id { get; set; }
    }
}
