using MediatR;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.Backend.Application.EssentialContext.Commands
{
    public class RegisterCommand : IRequest<TokenDto>
    {
        public RequestDto Request { get; }
        public RegisterDto Payload { get; }
        
        public RegisterCommand(RequestDto request, RegisterDto payload)
        {
            Request = request;
            Payload = payload;
        }
    }
}
