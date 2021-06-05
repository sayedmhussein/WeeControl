using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.ExtensionMethods
{
    public static class EntityValidationExtension
    {
        public static bool IsValid(this IDto dto)
        {
            return Validator.TryValidateObject(dto, new ValidationContext(dto), null, true);
        }

        [Obsolete]
        public static string ErrorMessage(this IDto dto)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateObject(dto, new ValidationContext(dto), results, true);
            return results?.FirstOrDefault()?.ErrorMessage;
        }

        public static ICollection<ValidationResult> GetErrorMessages(this IDto dto)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateObject(dto, new ValidationContext(dto), results, true);
            return results;
        }
    }
}
