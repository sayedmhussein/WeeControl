using System.Collections.Immutable;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.Dicts
{
    public interface IPersonalAttribDicts
    {
        ImmutableDictionary<PersonalTitleEnum, string> PersonTitle { get; }

        ImmutableDictionary<PersonalGenderEnum, string> PersonGender { get; }
    }
}
