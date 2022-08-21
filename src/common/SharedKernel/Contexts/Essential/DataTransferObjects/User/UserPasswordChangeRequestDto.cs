using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Common.SharedKernel.Contexts.Essential.DataTransferObjects.User;

public class UserPasswordChangeRequestDto
{
    [Required(ErrorMessage = "Old Password is required")]
    [StringLength(128, MinimumLength = 3, ErrorMessage = "Password length is between 3 and 128 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("Old Password")]
    public string OldPassword { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(128, MinimumLength = 6, ErrorMessage = "Password length is between 6 and 128 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("New Password")]
    public string NewPassword { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [NotMapped]
    public string ConfirmPassword { get; set; } = string.Empty;
}