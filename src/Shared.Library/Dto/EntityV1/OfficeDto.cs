using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dto.EntityV1
{
    public class OfficeDto : OfficeBase, IEntityDto
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }
    }
}
