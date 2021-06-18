using System;
using System.Collections.Generic;
using MediatR;

namespace WeeControl.Server.Application.Employee.Command.TerminateSession
{
    public class TerminateSessionCommand : IRequest
    {
        public ICollection<Guid> EmployeeIds { get; set; } = new List<Guid>();
    }
}
