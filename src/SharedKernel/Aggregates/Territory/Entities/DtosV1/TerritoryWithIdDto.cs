using System;
namespace WeeControl.SharedKernel.Aggregates.Territory.Entities.DtosV1
{
    public class TerritoryWithIdDto : BaseTerritory
    {
        public Guid Id { get; set; }

        public static implicit operator TerritoryWithIdDto(TerritoryDto v)
        {
            throw new NotImplementedException();
        }
    }
}
