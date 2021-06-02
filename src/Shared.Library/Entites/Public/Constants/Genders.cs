using System;
using System.Collections.Generic;

namespace MySystem.SharedKernel.Entities.Public.Constants
{
    public class Genders
    {
        public static IDictionary<Gender, string> List => new Dictionary<Gender, string>()
        {
            { Gender.Male, "m" },
            { Gender.Female, "f" }
        };

        public enum Gender
        {
            Male,
            Female
        }
    }
}
