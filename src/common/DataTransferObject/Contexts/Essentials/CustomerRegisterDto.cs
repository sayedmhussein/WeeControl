using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class CustomerRegisterDto
{
    public PersonModel Person { get; } = new();
    public UserModel User { get; } = new();
    public CustomerModel Customer { get; } = new();
}