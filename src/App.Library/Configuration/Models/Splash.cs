using System;
using Newtonsoft.Json;

namespace MySystem.Persistence.ClientService.Configuration.Models
{
    public class Splash
    {
        [JsonProperty("Disclaimer")]
        public string Disclaimer { get; set; }
    }
}
