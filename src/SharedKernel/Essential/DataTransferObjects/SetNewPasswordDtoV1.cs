namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class SetNewPasswordDtoV1
{
    public static SetNewPasswordDtoV1 Create(string oldPassword, string newPassword)
    {
        return new SetNewPasswordDtoV1() {OldPassword = oldPassword, NewPassword = newPassword};
    }
    
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }

    private SetNewPasswordDtoV1()
    {
    }
}