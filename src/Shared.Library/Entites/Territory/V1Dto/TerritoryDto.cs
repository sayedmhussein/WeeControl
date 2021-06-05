using System;
using MySystem.SharedKernel.Entities.Territory.Base;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entities.Territory.V1Dto
{
    public class TerritoryDto : TerritoryBase, IEntityDto
    {
        public Guid? Id { get; set; }
    }
}
