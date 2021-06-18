using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WeeControl.SharedKernel.CommonSchemas.Common.Dicts;
using WeeControl.SharedKernel.CommonSchemas.Employee.Enums;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.Dicts
{
    public class PersonalAttribDicts : BaseDicts, IPersonalAttribDicts
    {
        public ImmutableDictionary<PersonalTitleEnum, string> PersonTitle { get; private set; }

        public ImmutableDictionary<PersonalGenderEnum, string> PersonGender { get; private set; }

        public PersonalAttribDicts()
        {
            var personTitle = new Dictionary<PersonalTitleEnum, string>();
            foreach (var e in Enum.GetValues(typeof(PersonalTitleEnum)).Cast<PersonalTitleEnum>())
            {
                string value = obj.PersonalTitle[e.ToString()];
                personTitle.Add(e, value);
            }
            PersonTitle = personTitle.ToImmutableDictionary();

            var personGender = new Dictionary<PersonalGenderEnum, string>();
            foreach (var e in Enum.GetValues(typeof(PersonalGenderEnum)).Cast<PersonalGenderEnum>())
            {
                string value = obj.PersonalGender[e.ToString()];
                personGender.Add(e, value);
            }
            PersonGender = personGender.ToImmutableDictionary();
        }
    }
}
