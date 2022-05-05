using System;

namespace WeeControl.Common.SharedKernel.Interfaces
{
    public interface IIdentifyable
    {
        Guid Id { get; set; }
    }
}
