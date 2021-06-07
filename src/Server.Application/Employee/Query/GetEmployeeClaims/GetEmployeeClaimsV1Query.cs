using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace MySystem.Application.Employee.Query.GetEmployeeClaims
{
    public class GetEmployeeClaimsV1Query : IRequest<IEnumerable<Claim>>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Device { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
