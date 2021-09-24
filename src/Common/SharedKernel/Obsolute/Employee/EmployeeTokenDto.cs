
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.Obsolute.Employee
{
    public class EmployeeTokenDto : IDataTransferObject
    {
        public string Token { get; set; }

        public string FullName { get; set; }

        public string PhotoUrl { get; set; }

        public EmployeeTokenDto()
        {
        }

        public EmployeeTokenDto(string token, string fullName, string photoUrl)
        {
            Token = token;
            FullName = fullName;
            PhotoUrl = photoUrl;
        }
    }
}
