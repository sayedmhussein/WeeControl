using System;
namespace WeeControl.SharedKernel.Interfaces
{
    public interface IIdentifyable
    {
        Guid Id { get; set; }
    }
}
