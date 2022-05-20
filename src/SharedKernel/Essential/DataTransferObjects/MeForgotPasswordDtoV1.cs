using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class MeForgotPasswordDtoV1 : PasswordSetAbstractDto
{
    [Required(ErrorMessage = "Old Password is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Password length is between 3 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("Old Password")]
    public string OldPassword { get; set; }
}