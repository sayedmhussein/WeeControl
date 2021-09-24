using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public interface ITestsNotRequireAuthentication
    {
        void WhenSendingInvalidRequest_HttpResponseIsBadRequest();
    }
}