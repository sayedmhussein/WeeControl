using System;
using System.Collections.Generic;
using MediatR;

namespace WeeControl.Backend.Application.BoundContexts.Garbag.TerminateSessionV1
{
    public class TerminateSessionCommand : IRequest
    {
        public ICollection<Guid> EmployeeIds { get; set; } = new List<Guid>();
    }
}
