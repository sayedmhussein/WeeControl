namespace WeeControl.UserSecurityLib.Policies
{
    public static class PolicyGroup
    {
        
        
        public static class Authorization
        {
            public const string HasActiveSession = "HasSessionPolicy";
        }
        
        public static class Territory
        {
            public const string CanAlterTerritories = "CanAlterTerritories";
        }
        
        public static class Employee
        {
            public const string CanAddNewEmployee = "CanAddNewEmployee";
            public const string CanEditEmployeeDetails = "CanEditEmployeeDetails";
        }
    }
}