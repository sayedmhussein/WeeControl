using System;
namespace MySystem.Api.Dtos
{
    public class PortfolioBaseV1Dto
    {
        public Guid ContractId { get; set; }

        public Guid UnitId { get; set; }

        public Guid BuildingId { get; set; }

        public string ContractNo { get; set; }

        public string UnitNo { get; set; }

        public int UnitType { get; set; }

        public string BuildingName { get; set; }

        public string LastVisitedBy { get; set; }

        public DateTime? LastVisitedTs { get; set; }

        public DateTime? NextVisitTs { get; set; }
    }
}
