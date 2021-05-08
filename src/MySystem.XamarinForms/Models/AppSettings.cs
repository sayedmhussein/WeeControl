using System;
using MySystem.ClientService.Interfaces;

namespace MySystem.XamarinForms.Models
{
    public class AppSettings : IAppSettings
    {
        [Obsolete]
        public string WelComeText { get; set; }

        public string ApiBase { get; set; }
        public string ApiVersion { get; set; }
    }
}
