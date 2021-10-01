using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Server.Domain.Administration.ValueObjects;
using WeeControl.Server.Domain.Common.Interfaces;
using WeeControl.SharedKernel.Adminstration.Bases;

namespace WeeControl.Server.Domain.Administration.Entities
{
    public class Territory : TerritoryBase, IAggregateRoot
    {
        [Key]
        public string TerritoryCode { get; set; }

        public Territory ReportTo { get; set; }
        public string ReportToId { get; set; }
        
        public ICollection<Territory> Reporting { get; set; }

        public Address Address { get; set; }

        private Territory()
        {
        }

        public static Territory Create(string code, string name, string country, Address address, string reportToCode = null)
        {
            return new Territory()
            {
                TerritoryCode = code,
                Name = name, 
                CountryCode = country.ToUpper(), 
                ReportToId = reportToCode,
                Address = address ?? throw new ArgumentNullException("Address can't be null.")
            };
        }
    }
}