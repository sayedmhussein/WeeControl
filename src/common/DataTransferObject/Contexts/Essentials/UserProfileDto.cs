using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Core.DataTransferObject.Contexts.Essentials;

public class UserProfileDto
{
    public PersonModel Person { get; } = new();
    public UserModel User { get; } = new();
    public CustomerModel? Customer { get; set; } = null;

    public IEnumerable<AddressModel> Addresses { get; } = new List<AddressModel>();
    public IEnumerable<ContactModel> Contact { get; } = new List<ContactModel>();
}