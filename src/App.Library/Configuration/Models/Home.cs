using System;
using Newtonsoft.Json;

namespace MySystem.Persistence.ClientService.Configuration.Models
{
    public class Home
    {
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}
