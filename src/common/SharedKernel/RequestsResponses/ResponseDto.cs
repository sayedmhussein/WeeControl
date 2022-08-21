using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.RequestsResponses;

public class ResponseDto : IResponseDto
{
    public static ResponseDto Create(string debug = null!)
    {
        return new ResponseDto() { Debug = debug};
    }

    public static ResponseDto<T> Create<T>(T dto, string debug = null!) where T : class
    {
        return new ResponseDto<T>(dto, debug);
    }

    protected ResponseDto()
    {
        Debug = string.Empty;
    }

    protected ResponseDto(string debug)
    {
        Debug = debug;
    }

    public string Debug { get; set; }
}

public class ResponseDto<T> : ResponseDto, IResponseDto<T> where T : class
{
    public T Payload { get; set; }
    
    private ResponseDto() : base()
    {
        Payload = null!;
    }

    [Obsolete("Use static method to create response dto.")]
    public ResponseDto(T payload)
    {
        Payload = payload;
    }

    internal ResponseDto(T payload, string debug) : base(debug)
    {
        Payload = payload;
    }
}