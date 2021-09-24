using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.ValueObjects;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities
{
    public class Territory : IAggregateRoot
    {
        [Key]
        public string TerritoryCode { get; set; }

        public Territory ReportTo { get; set; }
        public string ReportToId { get; set; }
        
        public ICollection<Territory> Reporting { get; set; }

        public string CountryCode { get; set; }

        public string TerritoryName { get; set; }

        public Address Address { get; set; }

        private Territory()
        {
        }

        public static Territory Create(string code, string name, string country, Address address, string reportToCode = null)
        {
            return new Territory()
            {
                TerritoryCode = code,
                TerritoryName = name, 
                CountryCode = country.ToUpper(), 
                ReportToId = reportToCode,
                Address = address ?? throw new ArgumentNullException("Address can't be null.")
            };
        }
    }
}