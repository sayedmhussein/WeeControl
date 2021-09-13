using System;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.DtosV1.Territory
{
    public class IdentifiedTerritoryDto : TerritoryDto, IIdentifyable
    {
        public Guid Id { get; set; }
    }
}
