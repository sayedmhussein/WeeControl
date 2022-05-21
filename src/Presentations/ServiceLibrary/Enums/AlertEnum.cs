namespace WeeControl.Presentations.ServiceLibrary.Enums;

public enum AlertEnum
{
    DeveloperInvalidUserInput, DeveloperMinorBug,
    FailedToCommunicateWithServer,
    
    InvalidUsernameOrPassword, AccountIsLocked, PasswordUpdatedSuccessfully, InvalidPassword,
    SessionIsExpiredPleaseLoginAgain, NewPasswordSent,
    ExistingEmailOrUsernameExist, ExistingUsernameExist
    
}