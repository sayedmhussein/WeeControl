using System.Net;

namespace WeeControl.Common.BoundedContext.Interfaces
{
    public interface IResponseDto
    {
        HttpStatusCode StatuesCode { get; set; }
    }
}