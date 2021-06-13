using System;
using Newtonsoft.Json;

namespace MySystem.User.Employee.Configuration.Models
{
    public class Home
    {
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}
