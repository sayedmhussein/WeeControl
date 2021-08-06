using System;
namespace WeeControl.SharedKernel.Interfaces
{
    public interface IEntityDbo : IAggregateRoot
    {
        Guid Id { get; set; }
    }
}
