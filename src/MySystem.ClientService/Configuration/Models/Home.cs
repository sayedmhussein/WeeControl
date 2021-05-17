using System;
using Newtonsoft.Json;

namespace Sayed.MySystem.ClientService.Configuration.Models
{
    public class Home
    {
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}
