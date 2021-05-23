using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MySystem.Web.Api.Service
{
    public class ValidationService<T>
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage { get; private set; }

        public ValidationService(T item)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();

            IsValid = Validator.TryValidateObject(item, new ValidationContext(item), results, true);
            ErrorMessage = results?.FirstOrDefault()?.ErrorMessage;
        }
    }
}
