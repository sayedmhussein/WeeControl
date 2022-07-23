using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Essential.Models;

public class UserModel : IUserModel
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    public string SecondName { get; set; } = string.Empty;
    
    public string ThirdName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare(nameof(Password))]
    public string Password2 { get; set; } = string.Empty;
    
    public string MobileNo { get; set; } = string.Empty;
    
    public string TerritoryId { get; set; } = string.Empty;
    
    public string Nationality { get; set; } = string.Empty;
}