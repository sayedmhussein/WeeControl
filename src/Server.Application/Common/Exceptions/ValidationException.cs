using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Failures { get; }

        public ValidationException() : base("One or more validation failures have occurred.")
        {
        }

        public ValidationException(List<string> failures)
            : this()
        {
            
        }
    }
}
