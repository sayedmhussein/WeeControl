namespace WeeControl.User.UserServiceCore.Enums;

public enum AlertEnum
{
    DeveloperInvalidUserInput,
    DeveloperMinorBug,
    FailedToCommunicateWithServer,
    
    InvalidUsernameOrPassword, AccountIsLocked, PasswordUpdatedSuccessfully, InvalidPassword,
    SessionIsExpiredPleaseLoginAgain, NewPasswordSent,
    ExistingEmailOrUsernameExist, ExistingUsernameExist
    
}