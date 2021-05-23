using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySystem.Web.Api.Domain.User
{
    public interface IUserRepository
    {
        Task<Guid?> GetUserId(string username, string password);
        
        Task<Guid?> CreateOrGetSessionAsync(string username, string device);

        Task<bool> TerminateSessionAsync(Guid id);

        Task LogToSessionAsync(Guid session, string arg);

        //Task<IEnumerable<(DateTime datetime, string arg)>> GetSessionLogAsync(Guid id, DateTime? from = null, DateTime? until = null);
    }
}
