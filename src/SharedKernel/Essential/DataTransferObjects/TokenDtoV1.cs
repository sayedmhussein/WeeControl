namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class TokenDtoV1
{
    public string Token { get; set; }

    public string FullName { get; set; }

    public string PhotoUrl { get; set; }

    public TokenDtoV1()
    {
    }

    public TokenDtoV1(string token, string fullName, string photoUrl)
    {
        Token = token;
        FullName = fullName;
        PhotoUrl = photoUrl;
    }
}