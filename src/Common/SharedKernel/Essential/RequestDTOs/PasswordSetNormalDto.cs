namespace WeeControl.Common.SharedKernel.Essential.RequestDTOs;

public class PasswordSetNormalDto : PasswordSetAbstractDto
{
    public static class HttpPatchMethod
    {
        public const string EndPoint = "Api/Credentials/UpdateKPassword";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
    }
}