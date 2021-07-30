using System;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1
{
    public class TerritoryDto : BaseTerritory, IEntityDto
    {
        public Guid? Id { get; set; }

        public RequestMetadataV1 Metadata { get; set; }
    }
}
