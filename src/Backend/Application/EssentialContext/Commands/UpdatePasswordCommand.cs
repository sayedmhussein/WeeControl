using MediatR;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Backend.Application.EssentialContext.Commands;

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