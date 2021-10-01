using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Authorization.DtosV1
{
    public class RequestPasswordResetDto : IDataTransferObject
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username length is between 3 and 50 letters.")]
        [DisplayName("Username")]
        public string Username { get; set; }
        
        [MaxLength(45)]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}