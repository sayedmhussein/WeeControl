﻿using System;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Territory.BaseEntities
{
    public abstract class BaseTerritory : IVerifyable
    {
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Only ISO 3166-1 alpha-3 Country Codes.")]
        public string CountryId { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Office name must not exceed 45 character.")]
        public string Name { get; set; }

        public Guid? ReportToId { get; set; }
    }
}