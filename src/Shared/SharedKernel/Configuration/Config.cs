using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace MySystem.SharedKernel.Configuration
{
    public class Config
    {
        public static object AppSettingObject
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "MySystem.SharedKernel.Configuration.appsettings.json";
                using var stream = assembly.GetManifestResourceStream(resourceName);

                if (stream == null)
                    return null;

                using var streamReader = new StreamReader(stream);
                var jsonStream = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject(jsonStream);
            }
        }


    }
}
