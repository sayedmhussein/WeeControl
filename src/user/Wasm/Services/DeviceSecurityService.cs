// using System;
// using System.Threading.Tasks;
// using WeeControl.User.UserServiceCore.Enums;
// using WeeControl.User.UserServiceCore.Interfaces;
//
// namespace WeeControl.User.Wasm.Services;
//
// public class DeviceSecurityService : IDeviceSecurity
// {
//     private readonly IDeviceStorage storage;
//
//     public DeviceSecurityService(IDeviceStorage storage)
//     {
//         this.storage = storage;
//     }
//     
//     public bool IsAuthenticated()
//     {
//         throw new System.NotImplementedException();
//     }
//
//     public async Task UpdateTokenAsync(string token = null)
//     {
//         if (token is null)
//         {
//             await storage.ClearAsync();
//         }
//         else
//         {
//             await storage.SaveAsync(UserDataEnum.Token, token);
//         }
//     }
// }