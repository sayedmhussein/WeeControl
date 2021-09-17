namespace WeeControl.SharedKernel.Interfaces
{
    /// <summary>
    /// Interface for All Server Responses to Clients which host the Data Transfer Object.
    /// Server should send its response to client by classes which implement this interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponseDto<T> : IResponseDto where T : class
    {
        T Payload { get; set; }
    }
}
