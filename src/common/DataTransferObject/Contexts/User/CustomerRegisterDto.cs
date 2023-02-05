using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.DataTransferObject.Contexts.User;

public class CustomerRegisterDto
{
    public PersonModel Person { get; } = new();
    public UserModel User { get; } = new();
    public CustomerModel Customer { get; } = new();
}