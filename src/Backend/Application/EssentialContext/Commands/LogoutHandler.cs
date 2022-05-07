using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var session =
                await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == request.Sessionid && x.DeviceId == request.Request.DeviceId, cancellationToken);

            throw new NotImplementedException();
        }
    }
}
