namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class UserDetailedDtoV1 : UserDtoV1
{
    public IEnumerable<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
    public IEnumerable<SessionDto> Sessions { get; set; } = new List<SessionDto>();

}