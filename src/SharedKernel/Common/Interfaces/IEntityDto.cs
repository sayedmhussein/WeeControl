using System;
namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IEntityDto : IAggregateRoot
    {
        Guid? Id { get; set; }
    }
}
