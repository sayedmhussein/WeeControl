using System;
namespace Sayed.MySystem.Shared.Base
{
    public abstract class ContractUnitBase
    {
        public Guid ContractId { get; set; }

        public Guid UnitId { get; set; }

        public DateTime ActivationTs { get; set; }

        public DateTime? CancellationTs { get; set; }
    }
}
