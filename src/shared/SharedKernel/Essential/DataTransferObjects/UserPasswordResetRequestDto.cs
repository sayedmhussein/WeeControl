using WeeControl.SharedKernel.Essential.Entities;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class UserPasswordResetRequestDto : UserEntity
{
    public static UserPasswordResetRequestDto Create(string email, string username)
    {
        return new UserPasswordResetRequestDto()
        {
            Email = email, Username = username
        };
    }
}