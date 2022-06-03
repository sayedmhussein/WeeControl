
namespace WeeControl.SharedKernel.Interfaces;

/// <summary>
/// Interface for All Server Requests from Clients which host the Data Transfer Object.
/// Client should send their request to server by classes which implement this interface.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRequestDto<T> : IRequestDto where T : class
{
    T Payload { get; }
}