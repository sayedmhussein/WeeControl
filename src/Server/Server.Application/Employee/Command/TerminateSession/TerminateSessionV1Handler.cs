using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Interfaces;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Interfaces.Values;

namespace MySystem.Application.Employee.Command.TerminateSession
{
    public class TerminateSessionV1Handler : IRequestHandler<TerminateSessionV1Command>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo currentUser;
        private readonly ITerritoryValues sharedValues;

        public TerminateSessionV1Handler(IMySystemDbContext context, ICurrentUserInfo currentUser, ITerritoryValues sharedValues)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.sharedValues = sharedValues;
        }

        public async Task<Unit> Handle(TerminateSessionV1Command request, CancellationToken cancellationToken)
        {
            if (request.EmployeeIds.Any() == false)
            {
                var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == currentUser.SessionId, cancellationToken); ;
                request.EmployeeIds.Add(session.EmployeeId);
            }

            foreach (var employee in request.EmployeeIds)
            {
                var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.EmployeeId == employee && x.TerminationTs == null, cancellationToken);
                if (session != null)
                    session.TerminationTs = DateTime.UtcNow;
            }

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
