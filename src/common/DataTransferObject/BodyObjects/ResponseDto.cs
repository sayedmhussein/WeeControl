

namespace WeeControl.Core.DataTransferObject.BodyObjects;

public class ResponseDto
{
    public static ResponseDto Create(string debug = null!)
    {
        return new ResponseDto() { Debug = debug };
    }

    public static ResponseDto<T> Create<T>(T dto, string debug = null!) where T : class
    {
        return new ResponseDto<T>() { Debug = debug, Payload = dto };
    }

    protected ResponseDto()
    {
        Debug = string.Empty;
    }

    protected ResponseDto(string debug)
    {
        Debug = debug;
    }

    public string Debug { get; private init; }
}

public class ResponseDto<T> : ResponseDto where T : class
{
    public T Payload { get; set; }

    public ResponseDto()
    {
        Payload = null!;
    }
    
}