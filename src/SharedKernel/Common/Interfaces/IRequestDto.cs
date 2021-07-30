using WeeControl.SharedKernel.Common.DtosV1;

namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IRequestDto : IAggregateRoot
    {
        RequestMetadataV1 Metadata { get; set; }
    }
}
