using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Configurations;

namespace WeeControl.SharedKernel.BasicSchemas.Common.Dicts
{
    public abstract class BaseDicts
    {
        protected readonly dynamic obj;

        [Obsolete]
        public BaseDicts()
        {
            obj = GetSerializedAsJson(Settings.Filename, Assembly.GetExecutingAssembly());
        }

        public BaseDicts(Assembly assembly, string filename)
        {
            obj = GetSerializedAsJson(filename, assembly);
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
