using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Common.Extensions
{
    public static class EntityValidationExtension
    {
        public static bool IsValid(this IDto dto)
        {
            return Validator.TryValidateObject(dto, new ValidationContext(dto), null, true);
        }

        public static ICollection<ValidationResult> GetErrorMessages(this IDto dto)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateObject(dto, new ValidationContext(dto), results, true);
            return results;
        }
    }
}
