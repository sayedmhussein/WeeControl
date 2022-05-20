using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;

namespace WeeControl.Application.EssentialContext.Notifications;

public class UserActivityNotification : INotification
{
    private IRequest Request { get; }
    private string context { get; }
    private string details { get; }
    
    public UserActivityNotification(IRequest request)
    {
        Request = request;
    }
    
    public class UserActivityNotificationHandler : INotificationHandler<UserActivityNotification>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo currentUserInfo;

        public UserActivityNotificationHandler(IEssentialDbContext context, ICurrentUserInfo currentUserInfo)
        {
            this.context = context;
            this.currentUserInfo = currentUserInfo;
        }
        
        // protected override async void Handle(UserActivityNotification notification)
        // {
        //     var id = currentUserInfo.GetSessionId() ?? throw new NullReferenceException();
        //     var session = await context.Sessions.FirstAsync(x => x.SessionId == id);
        //     //var log = session.CreateLog();
        //     throw new System.NotImplementedException();
        // }
        
        public async Task Handle(UserActivityNotification notif, CancellationToken cancellationToken)
        {
            var id = currentUserInfo.GetSessionId() ?? throw new NullReferenceException();
            var session = await context.Sessions.FirstAsync(x => x.SessionId == id, cancellationToken);
            //var log = session.CreateLog();
            throw new System.NotImplementedException();
        }
    }
}