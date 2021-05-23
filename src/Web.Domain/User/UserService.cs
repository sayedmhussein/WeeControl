using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MySystem.Web.Api.Domain.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException();
        }

        public Task<IEnumerable<Claim>> GetUserClaims(Guid session)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid?> GetUserSessionAsync(string username, string password, string device)
        {
            if (await userRepository.GetUserId(username, password) != null)
            {
                return await userRepository.CreateOrGetSessionAsync(username, device);
            }
            else
            {
                return null;
            }
        }

        public Task LogToSession(Guid session, string str)
        {
            return userRepository.LogToSessionAsync(session, str);
        }

        public Task<bool> SessionIsValidAsync(Guid session)
        {
            throw new NotImplementedException();
        }

        public Task TerminateSessionAsync(Guid session)
        {
            throw new NotImplementedException();
        }
    }
}
