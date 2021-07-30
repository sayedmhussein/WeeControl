using WeeControl.SharedKernel.Common.DtosV1;

namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IRequestDto : IDto
    {
        RequestMetadataV1 Metadata { get; set; }
    }
}
