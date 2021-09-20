using System.Threading.Tasks;
using WeeControl.Common.SharedKernel.DtosV1.Employee;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Frontend.CommonLib.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IResponseDto> Login(CreateLoginDto dto);
        Task<IResponseDto> Refresh();
        Task<IResponseDto> Logout();
    }
}