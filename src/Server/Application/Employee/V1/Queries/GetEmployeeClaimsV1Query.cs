using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;

namespace WeeControl.Server.Application.Employee.Query.GetEmployeeClaims
{
    public class GetEmployeeClaimsV1Query : IRequest<IEnumerable<Claim>>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Guid? EmployeeId { get; set; }

        public IRequestMetadata Metadata { get; set; }
    }
}
