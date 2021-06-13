using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using MySystem.SharedKernel.Services;
using MySystem.User.Employee.Enumerators;
using MySystem.User.Employee.Interfaces;

namespace MySystem.User.Employee.Services
{
    public class EmployeeLibValues : IEmployeeLibValues
    {
        public ImmutableDictionary<ApplicationPageEnum, string> ApplicationPage { get; }

        public EmployeeLibValues()
        {
            dynamic obj = new EmbeddedResourcesManager(
                Assembly.GetExecutingAssembly())
                .GetSerializedAsJson("MySystem.User.Employee.Configuration.appsettings.json");

            var applicationPage = new Dictionary<ApplicationPageEnum, string>();
            foreach (var e in Enum.GetValues(typeof(ApplicationPageEnum)).Cast<ApplicationPageEnum>())
            {
                string value = obj.Pages[e.ToString()];
                applicationPage.Add(e, value);
            }
            ApplicationPage = applicationPage.ToImmutableDictionary();
        }
    }
}
