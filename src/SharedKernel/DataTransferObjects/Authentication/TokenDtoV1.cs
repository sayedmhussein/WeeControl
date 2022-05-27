namespace WeeControl.SharedKernel.DataTransferObjects.Authentication;

public class TokenDtoV1
{
    public static TokenDtoV1 Create(string token, string fullName, string photoUrl)
    {
        return new TokenDtoV1()
        {
            Token = token, FullName = fullName, PhotoUrl = photoUrl
        };
    }
    
    public string Token { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}