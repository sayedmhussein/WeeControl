using System.Net.Http;

namespace WeeControl.Common.SharedKernel
{
    public static class ApiRouteLink
    {
        public static class HumanResources
        {
            public const string Base = "http://10.0.2.2:5000/";

            #region Authorization
            public static class Authorization
            {
                public const string Route = "Api/Authorization/";
                
                public static class RequestNewToken
                {
                    public const string EndPoint = "";
                    public static readonly HttpMethod Method = HttpMethod.Post;
                    public const string Relative = Route + EndPoint;
                    public const string Absolute = Base + Relative;
                    public const string Version = "1.0";
                    
                }
                
                public static class RequestRefreshToken
                {
                    public const string EndPoint = "";
                    public static readonly HttpMethod Method = HttpMethod.Put;
                    public const string Relative = Route + EndPoint;
                    public const string Absolute = Base + Relative;
                    public const string Version = "1.0";
                    
                }
            }
            #endregion
            
            
            
        }
    }
}