using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.RequestDTOs;

public class PasswordSetForgottenDto : PasswordSetAbstractDto
{
    [Required(ErrorMessage = "Old Password is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Password length is between 3 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("Old Password")]
    public string OldPassword { get; set; }
    
    public static class HttpPatchMethod
    {
        public const string EndPoint = "Api/Credentials/UpdatePassword";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
    }
}