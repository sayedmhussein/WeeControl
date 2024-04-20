using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.DomainModel.Essentials.Dto;

public class UserProfileDto
{
    [ValidateComplexType] public PersonModel Person { get; } = new();

    [ValidateComplexType] public UserModel User { get; } = new();

    public ICollection<AddressModel> Addresses { get; } = new List<AddressModel>();
    public ICollection<ContactModel> Contact { get; } = new List<ContactModel>();
}