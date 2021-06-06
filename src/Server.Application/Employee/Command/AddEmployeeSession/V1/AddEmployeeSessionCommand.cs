using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace MySystem.Application.Employee.Command.AddEmployeeSession.V1
{
    public class AddEmployeeSessionCommand : IRequest<IEnumerable<Claim>>
    {
    }
}
