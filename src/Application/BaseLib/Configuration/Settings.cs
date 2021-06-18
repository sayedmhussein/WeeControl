using System;
namespace WeeControl.Applications.BaseLib.Configuration
{
    public static class Settings
    {
        public static string Filename => typeof(Settings).Namespace + ".appsettings.json";
    }
}
