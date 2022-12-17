using WeeControl.Common.SharedKernel.Contexts.Temporary.Entities;

namespace WeeControl.Common.SharedKernel.Contexts.Temporary.User;

public class RegisterEmployeeDto
{
    public PersonalEntity Personal { get; set; } = new ();

    public UserEntity User { get; set; } = new ();

    public EmployeeEntity Employee { get; set; } = new ();
}