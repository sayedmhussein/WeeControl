namespace WeeControl.Server.Test.Functional.TestHelpers
{
    public interface ITestsNotRequireAuthentication
    {
        void WhenSendingInvalidRequest_HttpResponseIsBadRequest();
    }
}