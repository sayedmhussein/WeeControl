using WeeControl.SharedKernel.Essential.RequestDTOs;

namespace WeeControl.SharedKernel.Essential.ResponseDTOs;

public class UserDetailedDto : UserDto
{
    public IEnumerable<ClaimDto> Claims { get; set; }
    public IEnumerable<SessionDto> Sessions { get; set; }
    
}