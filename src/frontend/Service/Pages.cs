using WeeControl.SharedKernel;

namespace WeeControl.Frontend.ApplicationService;

public static class Pages
{
    public static class Essential
    {
        public const string SplashPage = "Splash";
        public const string HomePage = "Index";
        public const string UserPage = "User";
    }

    public static class Elevator
    {
        public const string FieldPage = nameof(ClaimsValues.ClaimTypes.Field);
        public const string AdminPage = nameof(ClaimsValues.ClaimTypes.Administrator);
        public const string SalesPage = nameof(ClaimsValues.ClaimTypes.Sales);
    }
}