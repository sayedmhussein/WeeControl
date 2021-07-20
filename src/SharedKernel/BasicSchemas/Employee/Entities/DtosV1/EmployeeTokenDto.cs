using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.Entities.DtosV1
{
    public class EmployeeTokenDto : IDto
    {
        public string Token { get; set; }

        public string FullName { get; set; }
    }
}
