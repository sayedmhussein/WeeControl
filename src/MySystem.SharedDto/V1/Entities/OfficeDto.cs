using System;
using System.ComponentModel.DataAnnotations;
using Sayed.MySystem.SharedDto.BaseEntities;
using Sayed.MySystem.SharedDto.Interfaces;

namespace Sayed.MySystem.SharedDto.V1.Entities
{
    public class OfficeDto : OfficeBase
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }
    }
}
