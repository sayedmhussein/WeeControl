using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1.Common
{
    public class RequestDto<T> : RequestDto, IRequestDto<T> where T : class
    {
        public T Payload { get; set; }
    }
}
