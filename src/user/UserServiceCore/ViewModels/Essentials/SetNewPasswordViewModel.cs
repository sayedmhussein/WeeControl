using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.User.UserServiceCore.ViewModels.Essentials;

public class SetNewPasswordViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;

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
    public string ConfirmNewPassword { get; set; }
    
    public bool IsLoading { get; private set; }

    public SetNewPasswordViewModel(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task ChangeMyPassword()
    {
        IsLoading = true;
        await userService.UpdatePasswordAsync(SetNewPasswordDtoV1.Create(OldPassword, NewPassword));
        IsLoading = false;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}