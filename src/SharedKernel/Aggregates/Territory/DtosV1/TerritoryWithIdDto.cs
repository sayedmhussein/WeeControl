using System;
using WeeControl.SharedKernel.Aggregates.Territory.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Territory.DtosV1
{
    public class TerritoryWithIdDto : BaseTerritory, IAggregateRoot
    {
        public Guid Id { get; set; }

        public static implicit operator TerritoryWithIdDto(TerritoryDto v)
        {
            throw new NotImplementedException();
        }
    }
}
