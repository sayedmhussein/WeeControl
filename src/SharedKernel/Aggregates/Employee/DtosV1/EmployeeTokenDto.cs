
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Employee.DtosV1
{
    public class EmployeeTokenDto : IAggregateRoot
    {
        public string Token { get; set; }

        public string FullName { get; set; }
    }
}
