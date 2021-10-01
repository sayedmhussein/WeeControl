namespace WeeControl.Server.Test.Functional.Common
{
    public interface ITestsRequireAuthentication : ITestScenarios
    {
        void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized();

        void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden();
    }
}