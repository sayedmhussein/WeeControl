using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Frontend.ApplicationService.Essential.User;

public class PasswordChangeModel
{
    [Required(ErrorMessage = "Old Password is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Password length is between 3 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("Old Password")]
    public string OldPassword { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length is between 6 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("New Password")]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [NotMapped]
    public string ConfirmPassword { get; set; }
}