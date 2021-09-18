using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WeeControl.SharedKernel.DtosV1.Employee;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.CommonLib.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IResponseDto> Login(CreateLoginDto dto);
        Task<IResponseDto> Refresh();
        Task<IResponseDto> Logout();
    }
}