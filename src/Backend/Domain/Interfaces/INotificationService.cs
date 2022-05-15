using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeeControl.Domain.Interfaces;

public interface INotificationService
{
    Task SendAsync(IMessageDto message);
    Task SendAsync(IEnumerable<IMessageDto> messages);
}