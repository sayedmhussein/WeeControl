using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public interface ISecurityTestScenarios
    {
        [Fact]
        void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized();
        void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden();
    }
}