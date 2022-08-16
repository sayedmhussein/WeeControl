using WeeControl.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects;

public class RegisterEmployeeDto
{
    public PersonalEntity Personal { get; set; } = new ();

    public UserEntity User { get; set; } = new ();

    public EmployeeEntity Employee { get; set; } = new ();
}