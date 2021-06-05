using System;
using System.Collections.Generic;

namespace MySystem.SharedKernel.Entities.Public.Constants
{
    public class Titles
    {
        public static IDictionary<Title, string> List => new Dictionary<Title, string>()
        {
            { Title.Mr, "Mr." },
            { Title.Mrs, "Mrs." }
        };

        public enum Title
        {
            Mr,
            Mrs
        }
    }
}
