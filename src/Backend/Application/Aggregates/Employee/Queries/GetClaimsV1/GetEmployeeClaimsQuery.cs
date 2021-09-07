using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace WeeControl.Backend.Application.Aggregates.Employee.Queries.GetClaimsV1
{
    public class GetEmployeeClaimsQuery : IRequest<IEnumerable<Claim>>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Device { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
