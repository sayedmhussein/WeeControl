using WeeControl.Core.DataTransferObject.Contexts.Temporary.Entities;

namespace WeeControl.Core.DataTransferObject.Contexts.Temporary.User;

public class RegisterCustomerDto
{
    public PersonalEntity Personal { get; set; } = new();

    public UserEntity User { get; set; } = new();

    public CustomerEntity Customer { get; set; } = new();
}