using System.Net;

namespace WeeControl.Common.SharedKernel.Interfaces
{
    public interface IResponseDto
    {
        HttpStatusCode StatuesCode { get; set; }
    }
}