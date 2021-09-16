using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.DtosV1.Employee;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Frontend.CommonLib.DataAccess.Employee
{
    public class EmployeeData : IEmployeeData
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IDevice device;

        public EmployeeData(IHttpClientFactory clientFactory, IDevice device)
        {
            this.clientFactory = clientFactory;
            this.device = device;
        }
        
        public async Task<IResponseDto<EmployeeTokenDto>> GetToken(CreateLoginDto dto)
        {
            var client = clientFactory.CreateClient("NoAuth");
            var dto_ = new RequestDto<CreateLoginDto>() { DeviceId = device.DeviceId, Payload = dto };
            var response = await client.PostAsJsonAsync("/Api/Employee/Session", dto_);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var rDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();
                    rDto.HttpStatuesCode = (int)response.StatusCode;
                    return rDto;
                default:
                    var rDto_ = new ResponseDto<EmployeeTokenDto>(null);
                    rDto_.HttpStatuesCode = (int)response.StatusCode;
                    return rDto_;
            }
        }

        public Task<IResponseDto<EmployeeTokenDto>> GetToken(RefreshLoginDto dto)
        {
            throw new System.NotImplementedException();
        }
    }
}