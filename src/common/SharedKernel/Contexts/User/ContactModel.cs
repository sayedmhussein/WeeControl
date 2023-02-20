using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class ContactModel : IEntityModel
{
    public string ContactType { get; set; } = string.Empty;
    public string ContactValue { get; set; } = string.Empty;
}