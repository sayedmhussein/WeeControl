namespace WeeControl.Common.UserSecurityLib
{
    public static class SecurityPolicies
    {
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