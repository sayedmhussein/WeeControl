using System.Collections.Generic;
using System.Threading.Tasks;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.Server.Domain.Interfaces
{
    public interface INotificationService
    {
        Task SendAsync(IMessage message);
        Task SendAsync(IEnumerable<IMessage> messages);
    }
}
