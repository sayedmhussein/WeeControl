namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public interface ITestScenarios
    {
        void WhenSendingInvalidRequest_HttpResponseIsBadRequest();
        void WhenSendingValidRequest_HttpResponseIsSuccessCode();
    }
}