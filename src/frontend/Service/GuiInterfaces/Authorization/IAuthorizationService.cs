

namespace WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

[Obsolete]
public interface IAuthorizationService
{
    Task<bool> Login(string usernameOrEmail, string password);
    Task<bool> UpdateToken(string otp);
    Task<bool> Logout();

    string GetLabel(Label label);
    string GetMessage(Message message);

    enum Label { LoginHeader, LoginButton, OtpHeader, OtpButton, Username, Password, }

    enum Message
    {
        InvalidUsername, InvalidPassword, InvalidUsernameAndPassword,
        InvalidOtp, LockedUser
    }
}