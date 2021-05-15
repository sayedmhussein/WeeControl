using System.ComponentModel.DataAnnotations;

namespace Sayed.MySystem.Shared.Dtos.V1.Custom
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(3, ErrorMessage ="Minimum Password is 3 letters")]
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
