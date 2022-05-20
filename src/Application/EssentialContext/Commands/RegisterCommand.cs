using MediatR;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Commands;

public class RegisterCommand : IRequest<ResponseDto<TokenDtoV1>>
{
    public RequestDto Request { get; }
    public RegisterDtoV1 Payload { get; }
        
    public RegisterCommand(RequestDto request, RegisterDtoV1 payload)
    {
        Request = request;
        Payload = payload;
    }
}