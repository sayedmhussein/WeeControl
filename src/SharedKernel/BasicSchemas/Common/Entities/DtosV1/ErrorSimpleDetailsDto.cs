using System;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Common.Entities.DtosV1
{
    public class ErrorSimpleDetailsDto : IDto
    {
        public string Error { get; set; }
    }
}
