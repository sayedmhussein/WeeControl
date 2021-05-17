using System.ComponentModel.DataAnnotations;

namespace Sayed.MySystem.Shared.Dtos.V1.Custom
{
    public class LoginDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum Username is 3 letters")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Minimum Password is 3 letters")]
        public string Password { get; set; }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                    return false;
                else
                    return true;
            }
        }
    }
}
