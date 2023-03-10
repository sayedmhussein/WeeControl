using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.Essentials;

public class ContactModel : IEntityModel
{
    public enum ContactTypeEnum
    {
        Mobile,
        Home,
        Work,
        Fax,
        Email
    }

    [StringLength(25)] public string ContactType { get; set; } = string.Empty;

    [StringLength(55)] public string ContactValue { get; set; } = string.Empty;
}