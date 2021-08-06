using System;
using SQLite;
using WeeControl.SharedKernel.Aggregates.Territory.BaseEntities;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.Applications.BaseLib.Entities.Territory
{
    public class TerritoryDbo : BaseTerritory, IEntityDbo
    {
        [PrimaryKey]
        public Guid Id { get; set; }
    }
}
