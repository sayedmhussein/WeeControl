using System;
using SQLite;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Territory.Bases;

namespace WeeControl.Applications.BaseLib.Entities.Territory
{
    public class TerritoryDbo : BaseTerritory, IEntityDbo
    {
        [PrimaryKey]
        public Guid Id { get; set; }
    }
}
