using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Common.SharedKernel.Essential.RequestDTOs;

public class PasswordSetForgottenDto : PasswordSetAbstractDto
{
    public static class HttpPatchMethod
    {
        public const string EndPoint = "Api/Credentials/UpdatePassword";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
    }
}