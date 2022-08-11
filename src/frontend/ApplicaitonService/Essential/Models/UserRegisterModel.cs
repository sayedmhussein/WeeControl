using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Frontend.ApplicationService.Essential.Models;

public class UserRegisterModel 
{
    [Required]
    [StringLength(50)]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    public string SecondName { get; set; } = string.Empty;
    
    public string ThirdName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    [DisplayName("Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(3)]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    [DisplayName("Username")]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Compare(nameof(Password))]
    public string Password2 { get; set; } = string.Empty;
    
    [Phone]
    [DisplayName("Mobile Number")]
    public string MobileNo { get; set; } = string.Empty;
    
    [Required]
    [DisplayName("Territory")]
    public string TerritoryId { get; set; } = string.Empty;
    
    [Required]
    [DisplayName("Nationality")]
    public string Nationality { get; set; } = string.Empty;
}