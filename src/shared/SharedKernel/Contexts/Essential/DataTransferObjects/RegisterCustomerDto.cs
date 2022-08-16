using WeeControl.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects;

public class RegisterCustomerDto
{
    public PersonalEntity Personal { get; set; } = new ();

    public UserEntity User { get; set; } = new ();

    public CustomerEntity Customer { get; set; } = new ();
}