using WeeControl.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects.User;

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