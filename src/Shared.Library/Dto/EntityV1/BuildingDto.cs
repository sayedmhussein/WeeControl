using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dto.EntityV1
{
    public class BuildingDto : BuildingBase, IEntityDto
    {
        public Guid? Id { get; set; }  
    }
}
