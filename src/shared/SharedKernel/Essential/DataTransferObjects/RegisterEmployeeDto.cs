using WeeControl.SharedKernel.Essential.Entities;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class RegisterEmployeeDto
{
    public PersonalEntity Personal { get; set; } = new ();

    public UserEntity User { get; set; } = new ();

    public EmployeeEntity Employee { get; set; } = new ();
}