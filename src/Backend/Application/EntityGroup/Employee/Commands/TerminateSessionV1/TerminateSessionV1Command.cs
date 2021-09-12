using System;
using System.Collections.Generic;
using MediatR;

namespace WeeControl.Backend.Application.EntityGroup.Employee.Commands.TerminateSessionV1
{
    public class TerminateSessionCommand : IRequest
    {
        public ICollection<Guid> EmployeeIds { get; set; } = new List<Guid>();
    }
}
