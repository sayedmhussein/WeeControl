using System;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.Wasm.Services;

public class DeviceSecurityService : IDeviceSecurity
{
    
    
    public void TokenUpdated(string token)
    {
        throw new NotImplementedException();
    }

    public bool IsAuthenticated()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateToken(string token = null)
    {
        throw new System.NotImplementedException();
    }
}