using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.Core.Application.Contexts;

namespace WeeControl.Core.Application.Interfaces;

public interface INotificationService
{
    Task SendAsync(MessageDto message);
    Task SendAsync(IEnumerable<MessageDto> messages);
}