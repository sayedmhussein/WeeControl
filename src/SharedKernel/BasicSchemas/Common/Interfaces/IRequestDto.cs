﻿using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;

namespace WeeControl.SharedKernel.BasicSchemas.Common.Interfaces
{
    public interface IRequestDto : IDto
    {
        RequestMetadata Metadata { get; set; }
    }
}
