namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class SessionLogDto
{
    public DateTime LogTs { get; set; }

    public string Context { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;
}