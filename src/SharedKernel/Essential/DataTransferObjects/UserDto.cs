using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class UserDto
{
    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; }
    
    [Required]
    [MinLength(3)]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    [DisplayName("Username")]
    public string Username { get; set; }

    
    public string TerritoryCode { get; set; }
    
    public static class HttpGetMethod
    {
        public const string EndPoint = "Api/Essential/User";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
        
        public const string CanGetListOfUsersPolicy = "User_CanGetListOfUsers";
    }
    
    public static class HttpPutMethod
    {
        public const string EndPoint = "Api/Essential/User";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
        
        public const string CanEditUserPolicy = "User_CanAddNewEmployee";
    }
}