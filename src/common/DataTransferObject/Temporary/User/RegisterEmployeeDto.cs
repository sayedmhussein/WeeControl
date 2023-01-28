using WeeControl.Core.DataTransferObject.Temporary.Entities;

namespace WeeControl.Core.DataTransferObject.Temporary.User;

public class RegisterEmployeeDto
{
    public PersonalEntity Personal { get; set; } = new();

    public UserEntity User { get; set; } = new();

    public EmployeeEntity Employee { get; set; } = new();
}