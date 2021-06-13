using System;
using Newtonsoft.Json;

namespace MySystem.User.Employee.Configuration.Models
{
    public class Splash
    {
        [JsonProperty("Disclaimer")]
        public string Disclaimer { get; set; }
    }
}
