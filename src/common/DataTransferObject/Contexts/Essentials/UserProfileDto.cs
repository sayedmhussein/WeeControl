using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class UserProfileDto
{
    public PersonModel Person { get; set; } = new();
    public UserModel User { get; set; } = new();
    public ICollection<AddressModel> Addresses { get; set; } = new List<AddressModel>();
    public ICollection<ContactModel> Contact { get; set; } = new List<ContactModel>();
}