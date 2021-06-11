using System;
using System.Collections.Generic;
using MediatR;

namespace MySystem.Application.Employee.Command.TerminateSession
{
    public class TerminateSessionV1Command : IRequest
    {
        public ICollection<Guid> EmployeeIds { get; set; } = new List<Guid>();
    }
}
