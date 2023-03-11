namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class TokenResponseDto
{
    public string Token { get; set; } = string.Empty;

    [Obsolete("There is no need to send the name every time a token is received.")]
    public string FullName { get; set; } = string.Empty;

    public static TokenResponseDto Create(string token)
    {
        return new TokenResponseDto
        {
            Token = token
        };
    }

    [Obsolete("There is no need to send the name every time a token is received, use the overloaded function.")]
    public static TokenResponseDto Create(string token, string fullName)
    {
        return new TokenResponseDto
        {
            Token = token,
            FullName = fullName
        };
    }
}