namespace WeeControl.Core.DataTransferObject.Contexts.User;

public class HomeRequestDto
{
    public IEnumerable<Guid> ReadNotificationIds { get; set; }
}