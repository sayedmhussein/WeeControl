namespace WeeControl.Server.Test.Functional.Common
{
    public interface ITestsNotRequireAuthentication
    {
        void WhenSendingInvalidRequest_HttpResponseIsBadRequest();
    }
}