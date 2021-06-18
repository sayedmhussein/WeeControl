using System;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Territory.Bases;

namespace WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1
{
    public class TerritoryDto : BaseTerritory, IEntityDto
    {
        public Guid? Id { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
