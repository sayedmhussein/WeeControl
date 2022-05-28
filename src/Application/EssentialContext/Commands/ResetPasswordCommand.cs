using MediatR;
using WeeControl.SharedKernel.DataTransferObjects.User;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Commands;

public class ResetPasswordCommand : IRequest
{
    public IRequestDto Dto { get; }
    public string Email { get; }
    public string Username { get; }
    
    public ResetPasswordCommand(RequestDto<ForgotMyPasswordDtoV1> dto)
    {
        Dto = dto;
        Email = dto.Payload.Email;
        Username = dto.Payload.Username;
    }
}