using System;
using System.ComponentModel.DataAnnotations;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dtos.V1
{
    public class OfficeDto : OfficeBase
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }
    }
}
