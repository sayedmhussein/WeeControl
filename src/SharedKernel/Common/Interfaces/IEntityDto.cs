using System;
namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IEntityDto : IRequestDto
    {
        Guid? Id { get; set; }
    }
}
