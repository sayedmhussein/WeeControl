using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;

namespace WeeControl.SharedKernel.CommonSchemas.Common.Interfaces
{
    public interface IRequestDto : IDto
    {
        RequestMetadata Metadata { get; set; }
    }
}
