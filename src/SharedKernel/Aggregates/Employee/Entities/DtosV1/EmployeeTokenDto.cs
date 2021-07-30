using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.Entities.DtosV1
{
    public class EmployeeTokenDto : IDto
    {
        public string Token { get; set; }

        public string FullName { get; set; }
    }
}
