using System;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.Obsolutes.Dtos
{
    public class IdentifiedTerritoryDto : TerritoryDto, IIdentifyable
    {
        public Guid Id { get; set; }
    }
}
