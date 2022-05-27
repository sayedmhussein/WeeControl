namespace WeeControl.User.UserApplication.Test;

public class UnitTestTemplate : IDisposable
{
    #region Preparation
    private DeviceServiceMock mock;
    
    public UnitTestTemplate()
    {
        mock = new DeviceServiceMock(nameof(UnitTestTemplate));
    }
    
    public void Dispose()
    {
    }
    #endregion

    #region Success
    #endregion

    #region CommunicationFailure
    #endregion

    #region InvalidProperties
    #endregion

    #region InvalidCommands
    #endregion

    #region HttpCodes
    #endregion
}