namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class AuthenticationResponseDto
{
    public static AuthenticationResponseDto Create(string token, string fullName)
    {
        return new AuthenticationResponseDto()
        {
            Token = token, FullName = fullName
        };
    }
    
    public string Token { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
}