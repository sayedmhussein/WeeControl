using System;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entites.Employee.V1Dto
{
    public class EmployeeTerritoriesDto : IDto
    {
        public Guid TerritoryId { get; set; }
        public string TerritoryName { get; set; }
    }
}
