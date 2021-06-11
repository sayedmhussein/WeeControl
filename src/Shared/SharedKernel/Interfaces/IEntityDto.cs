using System;
namespace MySystem.SharedKernel.Interfaces
{
    public interface IEntityDto : IRequestDto
    {
        Guid? Id { get; set; }
    }
}
