using System;
using WeeControl.SharedKernel.EntityGroup.Territory.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.EntityGroup.Territory.DtosV1
{
    public class IdentifiedTerritoryDto : TerritoryDto, IIdentifyable
    {
        public Guid Id { get; set; }
    }
}
