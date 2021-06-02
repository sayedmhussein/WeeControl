using System;
namespace MySystem.SharedKernel.Interfaces
{
    public interface IEntityDto : IDto
    {
        Guid? Id { get; set; }
    }
}
