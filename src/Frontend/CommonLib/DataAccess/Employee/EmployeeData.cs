using System.Net.Http;
using WeeControl.Frontend.CommonLib.Interfaces;

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

    }
}