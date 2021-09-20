using System;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1.Territory
{
    public class IdentifiedTerritoryDto : TerritoryDto, IIdentifyable
    {
        public Guid Id { get; set; }
    }
}
