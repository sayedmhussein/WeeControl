using WeeControl.Core.DataTransferObject.Contexts.Temporary.User;
using WeeControl.Core.DataTransferObject.Contexts.User;

namespace WeeControl.Host.WebApiService.Contexts.User;

public interface IRegisterService
{
    Task RegisterCustomer(CustomerRegisterDto dto);
    Task ResisterEmployee(EmployeeRegisterDto dto);
}