namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class SessionDto
{
    public Guid SessionId { get; set; }
    
    public string DeviceId { get; set; } = string.Empty;

    public DateTime CreatedTs { get; set; }

    public DateTime? TerminationTs { get; set; }

    public IEnumerable<SessionLogDto> Logs { get; set; } = new List<SessionLogDto>();
}