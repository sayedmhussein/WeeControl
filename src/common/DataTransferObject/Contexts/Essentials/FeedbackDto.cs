using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using WeeControl.Core.DataTransferObject.BodyObjects;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class FeedbackDto : RequestDto
{
    [Required]
    public string FeedbackString { get; set; } = string.Empty;
    
    //[Required(ErrorMessage = "Please select files")]
    public List<IFormFile>? Files { get; set; } = new();
}