using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel.DtoParent;
using WeeControl.Core.SharedKernel.ExtensionHelpers;

namespace WeeControl.Core.Application.Contexts.Essentials.Commands;

public class SessionTerminateCommand : IRequest
{
    private readonly RequestDto request;

    public SessionTerminateCommand(RequestDto request)
    {
        this.request = request;
    }

    public class LogoutHandler : IRequestHandler<SessionTerminateCommand>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo currentUserInfo;

        public LogoutHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo)
        {
            this.context = context;
            this.currentUserInfo = currentUserInfo;
        }

        public async Task Handle(SessionTerminateCommand request, CancellationToken cancellationToken)
        {
            request.request.ThrowExceptionIfEntityModelNotValid();

            if (currentUserInfo.SessionId is null) throw new ArgumentNullException(nameof(currentUserInfo));

            var session =
                await context.UserSessions.FirstOrDefaultAsync(
                    x => x.SessionId == currentUserInfo.SessionId &&
                         x.DeviceId == request.request.DeviceId &&
                         x.TerminationTs == null, cancellationToken);

            if (session is not null)
            {
                session.TerminationTs = DateTime.UtcNow;
                await context.SessionLogs.AddAsync(
                    session.CreateLog("Logout", "Terminating session id:" + session.SessionId), cancellationToken);
            }
            else
            {
                throw new NotAllowedException("Already logged out!");
            }

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}