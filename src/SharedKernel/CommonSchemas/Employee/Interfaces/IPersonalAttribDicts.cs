using System.Collections.Immutable;
using WeeControl.SharedKernel.CommonSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.Dicts
{
    public interface IPersonalAttribDicts
    {
        ImmutableDictionary<PersonalTitleEnum, string> PersonTitle { get; }

        ImmutableDictionary<PersonalGenderEnum, string> PersonGender { get; }
    }
}
