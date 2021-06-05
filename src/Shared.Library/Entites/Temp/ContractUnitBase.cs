using System;
namespace MySystem.SharedKernel.EntityBase
{
    public abstract class ContractUnitBase
    {
        public Guid ContractId { get; set; }

        public Guid UnitId { get; set; }

        public DateTime ActivationTs { get; set; }

        public DateTime? CancellationTs { get; set; }
    }
}
