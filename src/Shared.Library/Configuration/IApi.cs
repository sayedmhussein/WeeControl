using System;

namespace MySystem.Shared.Library.Configuration
{
    public interface IApi
    {
        Uri Base { get; set; }
        string Version { get; set; }
        string Login { get; set; }
        string Token { get; set; }
        string Logout { get; set; }
    }
}