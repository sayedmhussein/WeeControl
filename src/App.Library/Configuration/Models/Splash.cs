using System;
using Newtonsoft.Json;

namespace MySystem.Web.ClientService.Configuration.Models
{
    public class Splash
    {
        [JsonProperty("Disclaimer")]
        public string Disclaimer { get; set; }
    }
}
