using System;
using MySystem.ClientService.Interfaces;

namespace MySystem.XamarinForms.Models
{
    public class AppSettings : IAppSettings
    {
        public string ApiBase { get; set; }
        public string ApiVersion { get; set; }

        public string LoginDisclaimer { get; set; }
        public string HomeDisclaimer { get; set; }
    }
}
