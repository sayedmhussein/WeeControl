namespace WeeControl.SharedKernel.BasicSchemas.Common.Interfaces
{
    public interface IMessage
    {
        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
    }
}
