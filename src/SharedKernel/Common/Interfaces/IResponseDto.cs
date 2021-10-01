using System.Net;

namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IResponseDto
    {
        HttpStatusCode StatuesCode { get; set; }
    }
}