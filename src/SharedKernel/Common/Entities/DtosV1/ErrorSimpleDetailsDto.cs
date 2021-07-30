using System;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.SharedKernel.Common.Entities.DtosV1
{
    public class ErrorSimpleDetailsDto : IDto
    {
        public string Error { get; set; }
    }
}
