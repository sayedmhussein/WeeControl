namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class UserDetailedDto : UserDto
{
    public IEnumerable<ClaimDto> Claims { get; set; }
    public IEnumerable<SessionDto> Sessions { get; set; }
    
}