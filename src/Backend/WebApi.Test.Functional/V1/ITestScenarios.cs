namespace WeeControl.Backend.WebApi.Test.Functional.V1
{
    public interface ITestScenarios
    {
        void WhenSendingInvalidRequest_HttpResponseIsBadRequest();
        void WhenSendingValidRequest_HttpResponseIsSuccessCode();
    }
}