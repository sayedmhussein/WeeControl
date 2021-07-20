using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.Entities.DtosV1
{
    public class RefreshLoginDto : IRequestDto
    {
        public RequestMetadataV1 Metadata { get; set; }
    }
}

