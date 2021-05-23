using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MySystem.Shared.Library.Base;

namespace MySystem.Shared.Library.Dtos.V1
{
    public class BuildingDto : BuildingBase, IDto
    {
        public Guid? Id { get; set; }  
    }
}
