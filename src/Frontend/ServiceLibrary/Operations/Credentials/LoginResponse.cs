using WeeControl.Frontend.ServiceLibrary.Enums;

namespace WeeControl.Frontend.ServiceLibrary.Operations.Credentials;

public class LoginResponse
{
    public bool IsSuccess { get; set; } = false;

    public UiResponseErrorCodeEnum ErrorCode { get; set; } = UiResponseErrorCodeEnum.NoError;

    public string ErrorMessage { get; set; } = String.Empty;
}