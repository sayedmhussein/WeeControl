using System;
using Newtonsoft.Json;

namespace MySystem.User.Employee.Configuration.Models
{
    public class Login
    {
        [JsonProperty("InvalidCredentialsMessage")]
        public string InvalidCredentialsMessage { get; set; }

        [JsonProperty("Disclaimer")]
        public string Disclaimer { get; set; }
    }
}
