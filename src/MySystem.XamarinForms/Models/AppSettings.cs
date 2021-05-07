using System;
using MySystem.ClientService.Interfaces;

namespace MySystem.XamarinForms.Models
{
    public class AppSettings : IAppSettings
    {
        public string WelComeText { get; set; }
    }
}
