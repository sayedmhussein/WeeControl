using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.NotificationModel;

namespace MySystem.Application.Employee.Command.CreateEmployee.V1
{
    public class EmployeeCreated : INotification
    {
        public Guid EmployeeId { get; set; }

        public class EmployeeCreatedHandler : INotificationHandler<EmployeeCreated>
        {
            private readonly INotificationService _notification;

            public EmployeeCreatedHandler(INotificationService notification)
            {
                _notification = notification;
            }

            public async Task Handle(EmployeeCreated notification, CancellationToken cancellationToken)
            {
                await _notification.SendAsync(new MessageDto());
            }
        }
    }
}
