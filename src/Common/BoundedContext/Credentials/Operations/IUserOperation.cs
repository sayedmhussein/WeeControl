using System;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.RequestsResponses;

namespace WeeControl.Common.BoundedContext.Credentials.Operations
{
    public interface IUserOperation
    {
        ResponseDto<TokenDto> Register(RequestDto<RegisterDto> dto);

        /// <summary>
        /// User can get token by sending username or email and password
        /// </summary>
        /// <param name="dto">Requst Dto of Type LoginDto</param>
        /// <returns>ResponseDto of type TokenDto</returns>
        ResponseDto<TokenDto> Login(RequestDto<LoginDto> dto);


    }
}
