using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.SharedKernel.Essential.RequestDTOs;

public abstract class PasswordSetAbstractDto
{
    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length is between 6 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("New Password")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [NotMapped]
    public string ConfirmNewPassword { get; set; }
}