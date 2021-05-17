using System;
using Newtonsoft.Json;

namespace Sayed.MySystem.ClientService.Configuration.Models
{
    public class Splash
    {
        [JsonProperty("Disclaimer")]
        public string Disclaimer { get; set; }
    }
}
