using System.IO;
using System.Reflection;
using MySystem.User.Employee.Configuration.Models;
using Newtonsoft.Json;

namespace MySystem.User.Employee.Configuration
{
    public class Config
    {
        [JsonProperty("Splash")]
        public Login Splash { get; set; }

        [JsonProperty("Login")]
        public Login Login { get; set; }

        [JsonProperty("Home")]
        public Home Home { get; set; }

        public static Config GetInstance()
        {
            var appsettingResouceStream = Assembly.GetAssembly(typeof(Config)).GetManifestResourceStream("MySystem.User.Employee.Configuration.values.json");
            if (appsettingResouceStream == null)
                return null;

            using var streamReader = new StreamReader(appsettingResouceStream);
            var jsonStream = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<Config>(jsonStream);
        }
    }
}
