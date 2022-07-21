namespace WeeControl.SharedKernel.Interfaces;

public interface IResponseDto
{
    public string Debug { get; }
}

public interface IResponseDto<T> : IResponseDto where T : class
{
    T Payload { get; }
}