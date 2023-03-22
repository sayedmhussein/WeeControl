using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class FeedbackDto
{
    [Required]
    public string FeedbackString { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please select files")]
    public List<IFormFile> Files { get; set; }
}