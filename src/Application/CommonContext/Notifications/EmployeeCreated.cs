using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Domain.Interfaces;

namespace WeeControl.Application.CommonContext.Notifications;

public class EmployeeCreated : INotification
{
    public Guid EmployeeId { get; set; }

    public class EmployeeCreatedHandler : INotificationHandler<EmployeeCreated>
    {
        private readonly IEmailNotificationService notification;

        public EmployeeCreatedHandler(IEmailNotificationService notification)
        {
            this.notification = notification;
        }

        public Task Handle(EmployeeCreated notif, CancellationToken cancellationToken)
        {
            //await _notification.SendAsync(new MessageDto());
            return Task.CompletedTask;
        }
    }
}