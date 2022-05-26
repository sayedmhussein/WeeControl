namespace WeeControl.User.UserServiceCore.Enums;

internal enum AlertEnum
{
    DeveloperInvalidUserInput,
    DeveloperMinorBug,
    FailedToCommunicateWithServer,
    
    InvalidUsernameOrPassword, AccountIsLocked, PasswordUpdatedSuccessfully, InvalidPassword,
    SessionIsExpiredPleaseLoginAgain, NewPasswordSent,
    ExistingEmailOrUsernameExist, ExistingUsernameExist
    
}