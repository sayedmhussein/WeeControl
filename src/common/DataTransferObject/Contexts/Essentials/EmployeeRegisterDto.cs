using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class EmployeeRegisterDto
{
    public PersonModel Person { get; } = new();
    public UserModel User { get; } = new();
    public EmployeeModel Employee { get; } = new();
}