using MediatR;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.EssentialContext.Commands;

public class ResetPasswordCommand : IRequest
{
    public IRequestDto Dto { get; }
    public string Email { get; }
    public string Username { get; }

    public ResetPasswordCommand(IRequestDto dto, string email, string username)
    {
        Dto = dto;
        Email = email;
        Username = username;
    }
}