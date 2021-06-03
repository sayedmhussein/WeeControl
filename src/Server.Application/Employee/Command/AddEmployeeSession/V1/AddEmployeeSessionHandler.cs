using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.SharedKernel.Entities.Public.Constants;
using MySystem.SharedKernel.ExtensionMethods;

namespace MySystem.Application.Employee.Command.AddEmployeeSession.V1
{
    public class AddEmployeeSessionHandler : IRequestHandler<AddEmployeeSessionCommand, IEnumerable<Claim>>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo currentUser;

        public AddEmployeeSessionHandler(IMySystemDbContext context, ICurrentUserInfo currentUser)
        {
            this.context = context;
            this.currentUser = currentUser;
        }

        public async Task<IEnumerable<Claim>> Handle(AddEmployeeSessionCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }



            if (request.Payload == null)// || string.IsNullOrWhiteSpace(request.DeviceId) || request.Payload.IsValid() == false)
            {
                throw new NotImplementedException();
            }
            else
            {
                var employee = await context.Employees.FirstOrDefaultAsync(x => x.Username == request.Payload.Username && x.Password == request.Payload.Password && x.AccountLockArgument == null, cancellationToken);

                if (employee == null)
                {
                    throw new NotFoundException(request.Payload.Username, employee?.Id);
                }

                var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Employee == employee && x.TerminationTs == null, cancellationToken);
                if (session == null)
                {
                    session = new();
                    session.EmployeeId = employee.Id;
                    session.DeviceId = request.DeviceId;
                    await context.EmployeeSessions.AddAsync(session, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }

                var claims = new List<Claim>()
            {
                new Claim(Claims.Types[Claims.ClaimType.Session], session.Id.ToString())
            };

                return claims;
            }
        }
    }
}
