using System;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.ValueObjects;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities
{
    public class Territory : IAggregateRoot
    {
        public Guid TerritoryId { get; set; }

        public Guid? ReportToId { get; set; }

        public string CountryCode { get; set; }

        public string TerritoryName { get; set; }

        public Address Address { get; set; }
    }
}