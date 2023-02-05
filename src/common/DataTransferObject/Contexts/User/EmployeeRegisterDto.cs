using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.DataTransferObject.Contexts.User;

public class EmployeeRegisterDto
{
    public PersonModel Person { get; } = new();

    public UserModel User { get; } = new();
}