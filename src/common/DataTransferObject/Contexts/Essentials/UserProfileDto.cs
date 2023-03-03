using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class UserProfileDto
{
    public PersonModel Person { get; } = new();
    public UserModel User { get; } = new();
    public CustomerModel? Customer { get; set; } = null;

    public ICollection<AddressModel> Addresses { get; } = new List<AddressModel>();
    public ICollection<ContactModel> Contact { get; } = new List<ContactModel>();
}