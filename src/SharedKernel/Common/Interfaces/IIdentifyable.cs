using System;

namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IIdentifyable
    {
        Guid Id { get; set; }
    }
}
