using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1
{
    public interface ISecurityTestScenarios
    {
        [Fact]
        void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized();
        void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden();
    }
}