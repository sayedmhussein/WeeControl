﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MySystem.SharedKernel.EntityBases.Employee
{
    public class EmployeeIdentityBase
    {
        public IdentityTypes IdentityType { get; set; }

        public string IdentityValue { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Only ISO 3166-1 alpha-3 Country Codes.")]
        public string CountryIssued { get; set; }

        public DateTime? DateIssed { get; set; }

        public DateTime? DateExpired { get; set; }

        public enum IdentityTypes
        {
            LocalId,
            Passport
        }
    }
}