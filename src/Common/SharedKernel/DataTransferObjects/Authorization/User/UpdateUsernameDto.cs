using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User
{
    public class UpdateEmailAsync
    {
        [Required]
        [DisplayName("New Username")]
        public string NewUserName { get; set; }
    }
}
