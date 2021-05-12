using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sayed.MySystem.SharedDto.BaseEntities;

namespace Sayed.MySystem.SharedDto.V1.Entities
{
    public class BuildingDto : BuildingBase
    {
        public Guid? Id { get; set; }  
    }
}
