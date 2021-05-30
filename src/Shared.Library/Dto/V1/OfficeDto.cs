using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interface;

namespace MySystem.SharedKernel.Dto.V1
{
    public class OfficeDto : TerritoryBase, IEntityDto
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }
    }
}
