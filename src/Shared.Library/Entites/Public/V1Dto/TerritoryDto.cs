using System;
using MySystem.SharedKernel.Entities.Base;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entities.V1Dto
{
    public class TerritoryDto : TerritoryBase, IEntityDto
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }
    }
}
