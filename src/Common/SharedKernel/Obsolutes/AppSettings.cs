using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace WeeControl.SharedKernel.Obsolutes
{
    [Obsolete]
    public abstract class AppSettings
    {
        protected readonly dynamic json;
        private string Filename => typeof(AppSettings).Namespace + ".appsettings.json";

        public AppSettings()
        {
            json = GetSerializedAsJson(Filename, Assembly.GetExecutingAssembly());
        }

        private object GetSerializedAsJson(string resource, Assembly assembly)
        {
            using var stream = assembly.GetManifestResourceStream(resource);

            if (stream == null)
                return null;

            using var streamReader = new StreamReader(stream);
            var jsonStream = streamReader.ReadToEnd();
            return JsonConvert.DeserializeObject(jsonStream);
        }
    }
}