using System;
using Newtonsoft.Json;

namespace Sayed.MySystem.ClientService.Models
{
    public class Home
    {
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}
