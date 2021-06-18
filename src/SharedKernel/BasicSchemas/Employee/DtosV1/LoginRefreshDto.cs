using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.SharedKernel.BasicSchemas.Employee.DtosV1
{
    public class RefreshLoginDto : IRequestDto
    {
        public RequestMetadata Metadata { get; set; }
    }
}

