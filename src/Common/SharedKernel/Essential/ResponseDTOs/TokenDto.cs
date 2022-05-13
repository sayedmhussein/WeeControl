namespace WeeControl.Common.SharedKernel.Essential.ResponseDTOs;

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
        
    public static class HttpPostMethod
    {
        public const string EndPoint = "Api/Credentials";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
    }
        
    public static class HttpPutMethod
    {
        public const string EndPoint = "Api/Credentials";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
    }
        
    public static class HttpDeleteMethod
    {
        public const string EndPoint = "Api/Credentials";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
    }
}