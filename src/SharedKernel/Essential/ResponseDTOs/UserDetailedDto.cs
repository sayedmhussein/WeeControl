using WeeControl.SharedKernel.Essential.RequestDTOs;

namespace WeeControl.SharedKernel.Essential.ResponseDTOs;

public class UserDetailedDto : UserDto
{
    public string Temp { get; set; }
}