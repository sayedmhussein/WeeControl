using WeeControl.Common.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.Common.SharedKernel.DataTransferObjects.User;

public class RegisterCustomerDto
{
    public PersonalEntity Personal { get; set; } = new ();

    public UserEntity User { get; set; } = new ();

    public CustomerEntity Customer { get; set; } = new ();
}