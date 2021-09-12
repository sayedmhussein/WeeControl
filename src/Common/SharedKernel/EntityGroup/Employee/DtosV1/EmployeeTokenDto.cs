
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroup.Employee.DtosV1
{
    public class EmployeeTokenDto : IEntityDto
    {
        public string Token { get; set; }

        public string FullName { get; set; }

        public string PhotoUrl { get; set; }
    }
}
