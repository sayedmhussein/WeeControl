using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.BoundedContexts.Credentials;

namespace WeeControl.Backend.Application.BoundContexts.Credentials.Commands
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly ICredentialsDbContext context;

        public LogoutHandler(ICredentialsDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var session =
                await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == request.Sessionid && x.DeviceId == request.Request.DeviceId, cancellationToken);

            throw new NotImplementedException();
        }
    }
}
