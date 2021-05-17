using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sayed.MySystem.Shared.Base;

namespace Sayed.MySystem.Shared.Dtos.V1.Entities
{
    public class BuildingDto : BuildingBase
    {
        public Guid? Id { get; set; }  
    }
}
