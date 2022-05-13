using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.DataTransferObjects.Essential.User
{
    public class UpdateEmailAsync
    {
        [Required]
        [EmailAddress]
        [DisplayName("New Email")]
        public string NewEmail { get; set; }
    }
}
