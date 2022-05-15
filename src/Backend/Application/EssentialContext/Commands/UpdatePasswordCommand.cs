using MediatR;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.EssentialContext.Commands;

public class UpdatePasswordCommand : IRequest
{
    public IRequestDto Request { get; }

    public string OldPassword { get; }
    public string NewPassword { get; }
    
    public UpdatePasswordCommand(IRequestDto dto, string oldPassword, string newPassword)
    {
        Request = dto;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}