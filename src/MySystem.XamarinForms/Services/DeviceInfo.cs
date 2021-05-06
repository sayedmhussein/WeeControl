using System;
using System.Net.Http;
using System.Threading.Tasks;
using MySystem.ClientService.Interfaces;
using MySystem.SharedDto.V1;
using Xamarin.Essentials;

namespace MySystem.XamarinForms.Services
{
    public class DeviceInfo : IDeviceInfo
    {
        public bool InternetIsAvailable => Connectivity.NetworkAccess == NetworkAccess.Internet;

    }
}
