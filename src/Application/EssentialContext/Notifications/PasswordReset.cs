using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Domain.Interfaces;

namespace WeeControl.Application.EssentialContext.Notifications;

public class PasswordReset : INotification
{
    public Guid UserId { get; }

    public string NewPassword { get; }

    public PasswordReset(Guid userid, string newPassword)
    {
        UserId = userid;
        NewPassword = newPassword;
    }
    
    public class PasswordResetHandler : INotificationHandler<PasswordReset>
    {
        private readonly IEmailNotificationService notification;

        public PasswordResetHandler(IEmailNotificationService notification)
        {
            this.notification = notification;
        }
        
        public Task Handle(PasswordReset notif, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}