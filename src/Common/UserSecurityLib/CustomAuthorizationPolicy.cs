namespace WeeControl.Common.UserSecurityLib
{
    public static class CustomAuthorizationPolicy
    {
        public const string IsAuthorized = "";

        public static class Territory
        {
            public const string CanAlterTerritories = "CanAlterTerritories";
        }
        
        public static class Employee
        {
            public const string CanAlterEmployee = "CanAddNewEmployee";
            public const string CanEditEmployeeDetails = "CanEditEmployeeDetails";
        }
    }
}