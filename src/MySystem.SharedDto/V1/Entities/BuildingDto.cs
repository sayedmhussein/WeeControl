using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MySystem.SharedDto.BaseEntities;

namespace MySystem.SharedDto.V1.Entities
{
    public class BuildingDto : BuildingBase
    {
        public Guid? Id { get; set; }  
    }
}
