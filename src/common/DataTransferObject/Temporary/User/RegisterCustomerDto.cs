using WeeControl.Core.DataTransferObject.Temporary.Entities;

namespace WeeControl.Core.DataTransferObject.Temporary.User;

public class RegisterCustomerDto
{
    public PersonalEntity Personal { get; set; } = new();

    public UserEntity User { get; set; } = new();

    public CustomerEntity Customer { get; set; } = new();
}