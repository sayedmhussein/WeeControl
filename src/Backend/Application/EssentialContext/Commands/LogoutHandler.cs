using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.Databases.Databases;

namespace WeeControl.Backend.Application.EssentialContext.Commands
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IEssentialDbContext context;

        public LogoutHandler(IEssentialDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (request.Sessionid is null)
            {
                throw new ArgumentNullException(nameof(request.Sessionid));
            }

            var session =
                await context.Sessions.FirstOrDefaultAsync(
                    x => x.SessionId == request.Sessionid && 
                         x.DeviceId == request.Request.DeviceId && 
                    x.TerminationTs == null, cancellationToken);

            if (session is not null)
            {
                session.TerminationTs = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NotAllowedException();
            }

            return Unit.Value;
        }
    }
}
