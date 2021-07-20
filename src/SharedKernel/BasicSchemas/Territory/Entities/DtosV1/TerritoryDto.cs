using System;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Territory.Entities.DtosV1
{
    public class TerritoryDto : BaseTerritory, IEntityDto
    {
        public Guid? Id { get; set; }

        public RequestMetadataV1 Metadata { get; set; }
    }
}
