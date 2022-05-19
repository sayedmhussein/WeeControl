namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class SessionLogDto
{
    public DateTime LogTs { get; set; }

    public string Context { get; set; }

    public string Details { get; set; }
}