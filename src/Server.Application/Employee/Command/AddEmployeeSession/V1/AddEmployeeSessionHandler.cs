using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Interfaces;

namespace MySystem.Application.Employee.Command.AddEmployeeSession.V1
{
    public class AddEmployeeSessionHandler : IRequestHandler<AddEmployeeSessionCommand, IEnumerable<Claim>>
    {
        public AddEmployeeSessionHandler(IMySystemDbContext context, ICurrentUserInfo currentUser)
        {
        }

        public Task<IEnumerable<Claim>> Handle(AddEmployeeSessionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
