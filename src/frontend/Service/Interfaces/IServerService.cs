using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Frontend.AppService.Interfaces;

public interface IServerService
{
    IResponseDto Send(IResponseDto dto);
    IResponseDto<T> Send<T>(IResponseDto dto) where T : class;
    
    IResponseDto Send<T>(IResponseDto<T> dto) where T : class;
    IResponseDto<T2> Send<T1, T2>(IResponseDto<T1> dto) where T1 : class where T2 : class;
}