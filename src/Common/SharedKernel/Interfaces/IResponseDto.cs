using System;
using System.Net;

namespace WeeControl.Common.SharedKernel.Interfaces
{
    public interface IResponseDto
    {
        [Obsolete]
        HttpStatusCode StatuesCode { get; set; }
    }
}