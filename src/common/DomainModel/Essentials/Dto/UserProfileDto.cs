namespace WeeControl.Core.DomainModel.Essentials.Dto;

public class UserProfileDto : PersonModel
{
    public ICollection<AddressModel> Addresses { get; } = new List<AddressModel>();
    public ICollection<ContactModel> Contact { get; } = new List<ContactModel>();
}