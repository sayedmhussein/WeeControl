using System;
using Newtonsoft.Json;

namespace MySystem.Persistence.ClientService.Configuration.Models
{
    public class Login
    {
        [JsonProperty("Disclaimer")]
        public string Disclaimer { get; set; }
    }
}
