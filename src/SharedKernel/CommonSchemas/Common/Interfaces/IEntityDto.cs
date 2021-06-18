using System;
namespace WeeControl.SharedKernel.CommonSchemas.Common.Interfaces
{
    public interface IEntityDto : IRequestDto
    {
        Guid? Id { get; set; }
    }
}
