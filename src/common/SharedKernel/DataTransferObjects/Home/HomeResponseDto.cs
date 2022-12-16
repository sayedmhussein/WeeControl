namespace WeeControl.Common.SharedKernel.DataTransferObjects.Home;

public class HomeResponseDto
{
    public IEnumerable<NotificationModel> Notifications { get; set; }
    public IEnumerable<FeedsModel> Feeds { get; set; }
    
    public record NotificationModel
    {
        public Guid Id { get; init; }
        public DateTime Ts { get; init; }
        public string Subject { get; init; } = string.Empty;
        public string Details { get; init; } = string.Empty;
    }

    public record FeedsModel
    {
        public DateTime Ts { get; init; }
        public string Subject { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
        public string Uri { get; init; } = string.Empty;
    }
}