namespace WeeControl.Server.Test.Functional.TestHelpers
{
    public interface ITestsRequireAuthentication : ITestScenarios
    {
        void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized();

        void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden();
    }
}