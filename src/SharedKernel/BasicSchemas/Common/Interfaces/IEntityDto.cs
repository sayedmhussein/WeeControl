using System;
namespace WeeControl.SharedKernel.BasicSchemas.Common.Interfaces
{
    public interface IEntityDto : IRequestDto
    {
        Guid? Id { get; set; }
    }
}
