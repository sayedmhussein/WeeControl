namespace WeeControl.SharedKernel.DataTransferObjects.User;

public class SetNewPasswordDtoV1
{
    public static SetNewPasswordDtoV1 Create(string oldPassword, string newPassword)
    {
        return new SetNewPasswordDtoV1() {OldPassword = oldPassword, NewPassword = newPassword};
    }

    public string NewPassword { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
}