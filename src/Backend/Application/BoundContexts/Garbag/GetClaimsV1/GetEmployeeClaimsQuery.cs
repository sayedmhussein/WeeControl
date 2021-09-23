using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace WeeControl.Backend.Application.BoundContexts.Garbag.GetClaimsV1
{
    public class GetEmployeeClaimsQuery : IRequest<IEnumerable<Claim>>
    {
        public string Device { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}
