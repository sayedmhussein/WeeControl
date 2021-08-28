using System;
namespace ClientLib
{
    public interface IDevice
    {
        string Token { get; set; }

        Uri ServerUri { get; }

    }
}
