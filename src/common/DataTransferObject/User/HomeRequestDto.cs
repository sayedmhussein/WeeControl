namespace WeeControl.Core.DataTransferObject.User;

public class HomeRequestDto
{
    public IEnumerable<Guid> ReadNotificationIds { get; set; }
}