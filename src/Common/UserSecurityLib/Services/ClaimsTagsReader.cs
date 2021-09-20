using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace WeeControl.Common.UserSecurityLib.Services
{
    public class ClaimsTagsReader
    {
        private readonly dynamic json;

        public ClaimsTagsReader(string fileLocation, string fileName, Assembly assembly = null)
        {
            json = GetSerializedAsJson(fileLocation + "." + fileName, assembly ?? Assembly.GetExecutingAssembly());
        }

        public void PopulateAttribute<TEnum, TValue>(ref Dictionary<TEnum, TValue> dictionary, string name)
        {
            if (dictionary != null) return;
            
            dictionary = new Dictionary<TEnum, TValue>();
            foreach (var e in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                TValue value = json[name][e.ToString()];
                dictionary.Add(e, value);
            }
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