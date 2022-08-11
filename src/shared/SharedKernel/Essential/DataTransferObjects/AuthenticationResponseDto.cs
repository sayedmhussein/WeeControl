namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class AuthenticationResponseDto
{
    public string Token { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
}