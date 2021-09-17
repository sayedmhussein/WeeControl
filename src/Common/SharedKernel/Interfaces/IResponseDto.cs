using System.Net;

namespace WeeControl.SharedKernel.Interfaces
{
    public interface IResponseDto
    {
        HttpStatusCode HttpStatuesCode { get; set; }
    }
}