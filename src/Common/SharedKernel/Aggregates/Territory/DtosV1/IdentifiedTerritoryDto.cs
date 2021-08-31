﻿using System;
using WeeControl.SharedKernel.Aggregates.Territory.BaseEntities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.Aggregates.Territory.DtosV1
{
    public class IdentifiedTerritoryDto : TerritoryDto, IIdentifyable
    {
        public Guid Id { get; set; }
    }
}
