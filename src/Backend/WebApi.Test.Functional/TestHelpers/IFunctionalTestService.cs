﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public interface IFunctionalTestService : IDisposable
    {
        Task<HttpResponseMessage> GetResponseMessageAsync(HttpRequestMessage requestMessage, string token = null);
        
        HttpContent GetHttpContentAsJson(ISerializable dto);
        
        Task<HttpRequestMessage> CloneRequestMessageAsync(HttpRequestMessage requestMessage = null);
        
        
        
        
        
        
        
        // HttpRequestMessage RequestMessage { get; }
        //
        // HttpClient Client { get; set; }
        //
        // string DeviceId { get; set; }
        //
        // Uri GetUri(ApiRouteEnum route);
        //
        // Task<HttpResponseMessage> GetResponseMessageAsync(Uri requestUri, HttpContent content = null, string token = null);
        //
    }
}
