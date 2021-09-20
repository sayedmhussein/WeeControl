
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1.Employee
{
    public class EmployeeTokenDto : IEntityDto
    {
        public string Token { get; set; }

        public string FullName { get; set; }

        public string PhotoUrl { get; set; }
    }
}
