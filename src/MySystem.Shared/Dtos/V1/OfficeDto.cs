using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.Shared.Base;

namespace Sayed.MySystem.Shared.Dtos.V1.Entities
{
    public class OfficeDto : OfficeBase
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }
    }
}
