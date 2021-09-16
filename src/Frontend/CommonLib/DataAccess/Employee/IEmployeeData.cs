using System.Threading.Tasks;
using WeeControl.SharedKernel.DtosV1.Employee;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.CommonLib.DataAccess.Employee
{
    public interface IEmployeeData
    {
        Task<IResponseDto<EmployeeTokenDto>> GetToken(CreateLoginDto dto);
        
        Task<IResponseDto<EmployeeTokenDto>> GetToken(RefreshLoginDto dto);
    }

    
}