using System;
using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;
using WeeControl.SharedKernel.CommonSchemas.Territory.Bases;

namespace WeeControl.SharedKernel.CommonSchemas.Territory.DtosV1
{
    public class TerritoryDto : BaseTerritory, IEntityDto
    {
        public Guid? Id { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
