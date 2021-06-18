using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using WeeControl.Applications.BaseLib.Configuration;
using WeeControl.Applications.BaseLib.Enumerators;
using WeeControl.Applications.BaseLib.Interfaces;

namespace WeeControl.User.Employee.Services
{
    public class EmployeeLibValues : IEmployeeLibValues
    {
        public ImmutableDictionary<ApplicationPageEnum, string> ApplicationPage { get; }

        public EmployeeLibValues()
        {
            dynamic obj = GetSerializedAsJson(Settings.Filename, Assembly.GetExecutingAssembly());

            var applicationPage = new Dictionary<ApplicationPageEnum, string>();
            foreach (var e in Enum.GetValues(typeof(ApplicationPageEnum)).Cast<ApplicationPageEnum>())
            {
                string value = obj.Pages[e.ToString()];
                applicationPage.Add(e, value);
            }
            ApplicationPage = applicationPage.ToImmutableDictionary();
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
