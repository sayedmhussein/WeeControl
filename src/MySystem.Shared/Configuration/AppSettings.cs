using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Sayed.MySystem.Shared.Configuration.Models;

namespace Sayed.MySystem.Shared.Configuration
{
    public class AppSettings
    {
        public static AppSettings GetAppSetting()
        {
            var stream = Assembly.GetAssembly(typeof(AppSettings)).GetManifestResourceStream("Sayed.MySystem.Shared.Configuration.appsettings.json");
            if (stream == null)
                return null;

            using var streamReader = new StreamReader(stream);
            var jsonStream = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<AppSettings>(jsonStream);
        }

        [JsonProperty("Api")]
        public Api Api { get; set; }
    }
}
