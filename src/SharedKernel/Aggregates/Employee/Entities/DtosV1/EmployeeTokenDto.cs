using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.Entities.DtosV1
{
    public class EmployeeTokenDto : IAggregateRoot
    {
        public string Token { get; set; }

        public string FullName { get; set; }
    }
}
