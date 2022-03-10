using System;
using System.Threading.Tasks;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.Interfaces;

namespace WeeControl.Common.BoundedContext.Credentials.Operations
{
    public class UserOperation
    {
        private IUserDevice device;

        public UserOperation(IUserDevice device)
        {
            this.device = device;
        }

        public Task<TokenDto> RegisterAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
