using System;
using MySystem.SharedKernel.EntityBases.Territory;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.EntityV1Dtos.Territory
{
    public class TerritoryDto : TerritoryBase, IEntityDto
    {
        public Guid? Id { get; set; }

        public RequestMetadata Metadata { get; set; }
    }
}
