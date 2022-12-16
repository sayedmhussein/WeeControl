namespace WeeControl.Common.SharedKernel.DataTransferObjects.Authentication;

public class TokenResponseDto
{
    public static TokenResponseDto Create(string token, string fullName)
    {
        return new TokenResponseDto()
        {
            Token = token, FullName = fullName
        };
    }
    
    public string Token { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
}