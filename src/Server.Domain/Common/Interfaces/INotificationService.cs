using System.Collections.Generic;
using System.Threading.Tasks;

namespace WeeControl.Server.Domain.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(IMessageDto message);
        Task SendAsync(IEnumerable<IMessageDto> messages);
    }
}
