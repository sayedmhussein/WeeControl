using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Contexts.Essentials.Queries;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Commands;

public class NotificationViewedCommand : IRequest
{
    private readonly Guid notificationId;

    public NotificationViewedCommand(Guid notificationId)
    {
        this.notificationId = notificationId;
    }

    public class NotificationViewedHandler : IRequestHandler<NotificationViewedCommand>
    {
        private readonly IEssentialDbContext essentialDbContext;
        private readonly ICurrentUserInfo userInfo;
        private readonly IMediator mediator;

        public NotificationViewedHandler(IEssentialDbContext essentialDbContext, ICurrentUserInfo userInfo, IMediator mediator)
        {
            this.essentialDbContext = essentialDbContext;
            this.userInfo = userInfo;
            this.mediator = mediator;
        }

        public async Task Handle(NotificationViewedCommand request, CancellationToken cancellationToken)
        {
            var notification = await essentialDbContext.UserNotifications.FirstOrDefaultAsync(
                x => x.NotificationId == request.notificationId, cancellationToken);

            if (notification is null)
            {
                throw new NotFoundException("No notification with this ID was found!");
            }

            var userId = await mediator.Send(new GetUserIdAndSessionVerificationQuery(), cancellationToken);
            
            if (userInfo.SessionId == null || notification.UserId != userId)
            {
                throw new NotAllowedException("User Not Found!");
            }

            if (notification.ReadTs is not null)
            {
                throw new NotAllowedException("Already was read");
            }

            notification.ReadTs = DateTime.UtcNow;
            await essentialDbContext.SaveChangesAsync(default);

            //return Unit.Value;
        }
    }
}