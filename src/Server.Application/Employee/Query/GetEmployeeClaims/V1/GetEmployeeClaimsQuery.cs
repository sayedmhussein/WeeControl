using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace MySystem.Application.Employee.Query.GetEmployeeClaims.V1
{
    public class GetEmployeeClaimsQuery : IRequest<IEnumerable<Claim>>
    {
        public Guid? EmployeeId { get; set; }

        public Guid? SessionId { get; set; }
    }
}
