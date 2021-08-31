
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.DtosV1
{
    public class EmployeeTokenDto : IEntityDto
    {
        public string Token { get; set; }

        public string FullName { get; set; }
    }
}
