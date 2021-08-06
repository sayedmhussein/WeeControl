﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Extensions
{
    public static class EntityValidationExtension
    {
        public static bool IsValid(this IAggregateRoot dto)
        {
            return Validator.TryValidateObject(dto, new ValidationContext(dto), null, true);
        }

        public static ICollection<ValidationResult> GetErrorMessages(this IAggregateRoot dto)
        {
            ICollection<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateObject(dto, new ValidationContext(dto), results, true);
            return results;
        }
    }
}
