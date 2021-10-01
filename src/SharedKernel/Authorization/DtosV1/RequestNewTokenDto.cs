using WeeControl.SharedKernel.Authorization.Bases;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Authorization.DtosV1
{
    public class RequestNewTokenDto : UserBase, IDataTransferObject
    {
        public RequestNewTokenDto()
        {
        }
        
        public RequestNewTokenDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
