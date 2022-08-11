using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class UserPasswordChangeRequestDto
{
    [Required]
    [StringLength(128, MinimumLength = 6)]
    public string OldPassword { get; set; } = string.Empty;
    
    [Required]
    [StringLength(128, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;
    
    [Required]
    [StringLength(128, MinimumLength = 6)]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}