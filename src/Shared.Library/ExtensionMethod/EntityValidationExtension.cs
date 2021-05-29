using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MySystem.SharedKernel.Interface;

namespace MySystem.SharedKernel.ExtensionMethod
{
    public static class EntityValidationExtension
    {
        public static bool IsValid(this IEntityDto dto)
        {
            return Validator.TryValidateObject(dto, new ValidationContext(dto), null, true);
        }

        public static string ErrorMessage(this IEntityDto dto)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateObject(dto, new ValidationContext(dto), results, true);
            return results?.FirstOrDefault()?.ErrorMessage;
        }
    }
}
