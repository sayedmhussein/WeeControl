namespace WeeControl.Common.BoundedContext.Credentials.DataTransferObjects
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
        }
    }
}
