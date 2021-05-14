using System;
using Newtonsoft.Json;

namespace Sayed.MySystem.ClientService.Models
{
    public class Login
    {
        [JsonProperty("Disclaimer")]
        public string Disclaimer { get; set; }
    }
}
