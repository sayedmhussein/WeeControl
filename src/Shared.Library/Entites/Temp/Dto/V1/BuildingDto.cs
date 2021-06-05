using System;
using MySystem.SharedKernel.EntityBase;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Dto.V1
{
    public class BuildingDto : BuildingBase, IEntityDto
    {
        public Guid? Id { get; set; }  
    }
}
