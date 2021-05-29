using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace MySystem.SharedKernel.Configuration
{
    public class Api : IApi
    {
        public static Api GetAppSetting()
        {
            var stream = Assembly.GetAssembly(typeof(Api)).GetManifestResourceStream("MySystem.Shared.Library.Configuration.apisetting.json");
            if (stream == null)
                return null;

            using var streamReader = new StreamReader(stream);
            var jsonStream = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<Api>(jsonStream);
        }

        [JsonProperty("Base")]
        public Uri Base { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("Login")]
        public string Login { get; set; }

        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("Logout")]
        public string Logout { get; set; }
    }
}
