using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;
using WeeControl.SharedKernel.CommonSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.CommonSchemas.Employee.DtosV1
{
    public class RefreshLoginDto : IRequestDto
    {
        public RequestMetadata Metadata { get; set; }
    }
}

