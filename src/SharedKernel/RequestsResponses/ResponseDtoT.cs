using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.RequestsResponses;

public class ResponseDto<T> : ResponseDto, IResponseDto<T> where T : class
{
    public T Payload { get; set; }
    
    private ResponseDto() : base()
    {
    }

    public ResponseDto(T payload)
    {
        Payload = payload;
    }
}