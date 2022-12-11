using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.AppService.Interfaces;

namespace WeeControl.Frontend.AppService.Services;

public class ServerService : IServerService
{
    public IResponseDto Send(IResponseDto dto)
    {
        throw new NotImplementedException();
    }

    public IResponseDto<T> Send<T>(IResponseDto dto) where T : class
    {
        throw new NotImplementedException();
    }

    public IResponseDto Send<T>(IResponseDto<T> dto) where T : class
    {
        throw new NotImplementedException();
    }

    public IResponseDto<T2> Send<T1, T2>(IResponseDto<T1> dto) where T1 : class where T2 : class
    {
        throw new NotImplementedException();
    }
}