using System;
using System.Collections.Generic;

namespace MySystem.SharedKernel.Entities.Public.Constants
{
    public class Counties
    {
        public static IDictionary<Name, string> List => new Dictionary<Name, string>()
        {
            { Name.USA, "usa" },
            { Name.EGYPT, "egy" },
            { Name.Saudia, "sau" }
        };

        public enum Name
        {
            USA,
            EGYPT,
            Saudia
        }
    }
}
