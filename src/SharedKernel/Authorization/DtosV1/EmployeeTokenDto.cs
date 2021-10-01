namespace WeeControl.SharedKernel.Authorization.DtosV1
{
    public class EmployeeTokenDto
    {
        public string Token { get; set; }

        public string FullName { get; set; }

        public string PhotoUrl { get; set; }

        public EmployeeTokenDto()
        {
        }

        public EmployeeTokenDto(string token, string fullName, string photoUrl) : this()
        {
            Token = token;
            FullName = fullName;
            PhotoUrl = photoUrl;
        }
    }
}