﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySystem.ClientService.Interfaces
{
    public interface IDeviceResources
    {
        Task<HttpClient> GetHttpClientAsync();

        Task SaveTokenAsync(string token);
    }
}
