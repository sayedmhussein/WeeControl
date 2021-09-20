using System.Net;

namespace WeeControl.Common.SharedKernel.Interfaces
{
    public interface IResponseDto
    {
        HttpStatusCode HttpStatuesCode { get; set; }
    }
}