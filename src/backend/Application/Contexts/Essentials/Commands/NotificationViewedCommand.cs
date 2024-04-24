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

public class NotificationViewedCommand(Guid notificationId) : IRequest
{
    private readonly Guid notificationId = notificationId;

    public class NotificationViewedHandler(
        IEssentialDbContext essentialDbContext,
        ICurrentUserInfo userInfo,
        IMediator mediator)
        : IRequestHandler<NotificationViewedCommand>
    {
        public async Task Handle(NotificationViewedCommand request, CancellationToken cancellationToken)
        {
            var notification = await essentialDbContext.UserNotifications.FirstOrDefaultAsync(
                x => x.NotificationId == request.notificationId, cancellationToken);

            if (notification is null) throw new NotFoundException("No notification with this ID was found!");

            var userId = await mediator.Send(new GetUserIdAndSessionVerificationQuery(), cancellationToken);

            if (userInfo.SessionId == null || notification.UserId != userId)
                throw new NotAllowedException("User Not Found!");

            if (notification.ReadTs is not null) throw new NotAllowedException("Already was read");

            notification.ReadTs = DateTime.UtcNow;
            await essentialDbContext.SaveChangesAsync(default);
        }
    }
}