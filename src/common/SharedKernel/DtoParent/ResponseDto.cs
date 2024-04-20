namespace WeeControl.Core.SharedKernel.DtoParent;

public class ResponseDto
{
    protected ResponseDto()
    {
        Debug = string.Empty;
    }

    protected ResponseDto(string debug)
    {
        Debug = debug;
    }

    public string Debug { get; private init; }

    public static ResponseDto Create(string debug = null!)
    {
        return new ResponseDto {Debug = debug};
    }

    public static ResponseDto<T> Create<T>(T dto, string debug = null!) where T : class
    {
        return new ResponseDto<T> {Debug = debug, Payload = dto};
    }
}

public class ResponseDto<T> : ResponseDto where T : class
{
    public ResponseDto()
    {
        Payload = null!;
    }

    public T Payload { get; set; }
}