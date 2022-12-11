using WeeControl.Common.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.Common.SharedKernel.DataTransferObjects.User;

public class RegisterEmployeeDto
{
    public PersonalEntity Personal { get; set; } = new ();

    public UserEntity User { get; set; } = new ();

    public EmployeeEntity Employee { get; set; } = new ();
}