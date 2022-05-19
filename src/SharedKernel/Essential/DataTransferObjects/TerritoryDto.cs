namespace WeeControl.SharedKernel.Essential.DataTransferObjects;

public class TerritoryDto
{
    public string TerritoryCode { get; set; }
    
    public string ReportToId { get; set; }

    public string CountryCode { get; set; }

    public string TerritoryName { get; set; }
    
    public static class HttpGetMethod
    {
        public const string EndPoint = "Api/Essential/User";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
        
        public const string CanGetListOfTerritores = "User_CanGetListOfTerritores";
    }
    
    public static class HttpPutMethod
    {
        public const string EndPoint = "Api/Essential/User";
        public const string Version = "1.0";
        public static string AbsoluteUri(string server) => server + EndPoint;
        
        public const string CanEditTerritoryPolicy = "User_CanAddNewEmployee";
    }
}