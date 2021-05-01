using System;
using System.ComponentModel.DataAnnotations;
using MySystem.SharedDto.BaseEntities;
using MySystem.SharedDto.Interfaces;

namespace MySystem.SharedDto.V1.Entities
{
    public class OfficeDto : OfficeBase
    {
        public Guid? Id { get; set; }

        public Guid? ParentId { get; set; }
    }
}
