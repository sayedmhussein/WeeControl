using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace MySystem.SharedKernel.Services
{
    public class EmbeddedResourcesManager
    {
        private readonly Assembly assembly;

        public EmbeddedResourcesManager(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public object GetSerializedAsJson(string resource)
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
