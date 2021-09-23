using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces;

namespace WeeControl.Backend.Application.BoundContexts.Garbag.TerminateSessionV1
{
    public class TerminateSessionV1Handler : IRequestHandler<TerminateSessionCommand>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo currentUser;
        private readonly ITerritoryAttribute sharedValues;

        public TerminateSessionV1Handler(IMySystemDbContext context, ICurrentUserInfo currentUser, ITerritoryAttribute sharedValues)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.sharedValues = sharedValues;
        }

        public async Task<Unit> Handle(TerminateSessionCommand request, CancellationToken cancellationToken)
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
