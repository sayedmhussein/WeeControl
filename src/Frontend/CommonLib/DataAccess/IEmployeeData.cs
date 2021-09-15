using System.Threading.Tasks;
using Refit;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.DtosV1.Employee;
using WeeControl.SharedKernel.Routing;

namespace WeeControl.Frontend.CommonLib.DataAccess
{
    public interface IEmployeeData
    {
        [Post("/Api/Employee/Session")]
        Task<ResponseDto<EmployeeTokenDto>> GetToken(RequestDto<CreateLoginDto> dto);
        
        [Put("/Api/Employee/Session")]
        Task<ResponseDto<EmployeeTokenDto>> GetToken(RequestDto<RefreshLoginDto> dto);
    }
}