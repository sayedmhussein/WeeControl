using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class UserProfileDto
{
    [ValidateComplexType] 
    public PersonModel Person { get; set; } = new();
    
    [ValidateComplexType] 
    public UserModel User { get; set; } = new();
    public ICollection<AddressModel> Addresses { get; set; } = new List<AddressModel>();
    public ICollection<ContactModel> Contact { get; set; } = new List<ContactModel>();
}