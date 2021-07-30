using System;
namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IEntityDbo : IAggregateRoot
    {
        Guid Id { get; set; }
    }
}
