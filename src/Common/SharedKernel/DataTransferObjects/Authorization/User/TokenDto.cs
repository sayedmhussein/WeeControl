namespace WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User
{
    public class TokenDto
    {
        public string Token { get; set; }

        public string FullName { get; set; }

        public string PhotoUrl { get; set; }

        public TokenDto()
        {
        }

        public TokenDto(string token, string fullName, string photoUrl)
        {
            Token = token;
            FullName = fullName;
            PhotoUrl = photoUrl;
        }
    }
}
