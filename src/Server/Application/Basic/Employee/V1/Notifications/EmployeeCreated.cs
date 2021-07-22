using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Domain.Interfaces;

namespace WeeControl.Server.Application.Employee.Command.AddEmployee.V1
{
    public class EmployeeCreated : INotification
    {
        public Guid EmployeeId { get; set; }

        public class EmployeeCreatedHandler : INotificationHandler<EmployeeCreated>
        {
            private readonly IEmailNotificationService _notification;

            public EmployeeCreatedHandler(IEmailNotificationService notification)
            {
                _notification = notification;
            }

            public Task Handle(EmployeeCreated notification, CancellationToken cancellationToken)
            {
                //await _notification.SendAsync(new MessageDto());
                return Task.CompletedTask;
            }
        }
    }
}
