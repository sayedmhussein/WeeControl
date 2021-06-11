using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Application.Employee.Query.GetEmployeeClaims
{
    public class GetEmployeeClaimsV1Query : IRequest<IEnumerable<Claim>>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        [Obsolete]
        public string Device => Metadata?.Device;

        public Guid? EmployeeId { get; set; }

        public IRequestMetadata Metadata { get; set; }
    }
}
