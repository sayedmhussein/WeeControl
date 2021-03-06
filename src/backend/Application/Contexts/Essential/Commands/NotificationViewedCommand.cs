using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;

namespace WeeControl.Application.Contexts.Essential.Commands;

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

        public NotificationViewedHandler(IEssentialDbContext essentialDbContext, ICurrentUserInfo userInfo)
        {
            this.essentialDbContext = essentialDbContext;
            this.userInfo = userInfo;
        }
        
        public async Task<Unit> Handle(NotificationViewedCommand request, CancellationToken cancellationToken)
        {
            var item = await essentialDbContext.UserNotifications.FirstOrDefaultAsync(
                x => x.NotificationId == request.notificationId, cancellationToken);

            if (item is null)
            {
                throw new NullReferenceException();
            }
            
            item.ViewedTs = DateTime.UtcNow;
            await essentialDbContext.SaveChangesAsync(default);

            return Unit.Value;
        }
    }
}