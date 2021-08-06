using System;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.DtosV1
{
    public class ErrorSimpleDetailsDto : IAggregateRoot
    {
        public string Error { get; set; }
    }
}
