using MediatR;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Commands;

public class RegisterCommand : IRequest<TokenDtoV1>
{
    public RequestDto Request { get; }
    public RegisterDto Payload { get; }
        
    public RegisterCommand(RequestDto request, RegisterDto payload)
    {
        Request = request;
        Payload = payload;
    }
}