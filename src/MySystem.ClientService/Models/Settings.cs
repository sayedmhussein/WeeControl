using System;
using Newtonsoft.Json;
using Sayed.MySystem.ClientService.Models;

namespace Sayed.MySystem.ClientService.Services
{
    public class Setting
    {
        [JsonProperty("Api")]
        public Api Api { get; set; }

        [JsonProperty("Splash")]
        public Login Splash { get; set; }

        [JsonProperty("Login")]
        public Login Login { get; set; }

        [JsonProperty("Home")]
        public Home Home { get; set; }
    }
}
