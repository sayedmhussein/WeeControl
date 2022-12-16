namespace WeeControl.Common.SharedKernel.DataTransferObjects.Home;

public class HomeRequestDto
{
    public IEnumerable<Guid> ReadNotificationIds { get; set; }
}