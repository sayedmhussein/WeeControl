using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Server.Domain.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(IMessageDto message);
        Task SendAsync(IEnumerable<IMessageDto> messages);
    }
}
