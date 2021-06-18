//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Xunit;

//namespace MySystem.Web.Api.Test.Controller.V1.Employee.Credentials
//{
//    public class LoginTests : IClassFixture<WebApplicationFactory<Startup>>
//    {
//        internal static async Task<string> GetTokenAsValidUserAsync(HttpClient client)
//        {
//            var loginUrl = new ApiService().Route[RouteEnum.EmployeeSession];

//            var loginDto = new LoginDto() { Username = "admin", Password = "admin" };
//            var requestDto = new RequestDto<LoginDto>("DeviceId", loginDto);

//            var response = await client.PostAsJsonAsync(loginUrl, requestDto);

//            response.EnsureSuccessStatusCode();

//            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();
//            return responseDto.Payload;
//        }

//        private readonly WebApplicationFactory<Startup> factory;
//        private readonly string loginUrl;

//        public LoginTests(WebApplicationFactory<Startup> factory)
//        {
//            this.factory = factory;
//            loginUrl = new ApiService().Route[RouteEnum.EmployeeSession];
//        }

//        [Fact]
//        public async void WhenValidCredentials_NewTokenCreated()
//        {
//            var client = factory.CreateClient();
//            var token = await GetTokenAsValidUserAsync(client);
//            Assert.NotEmpty(token);
//        }

//        [Theory]
//        [ClassData(typeof(LoginTestsClassData))]
//        public async void LoginTheoryTests(object requestDto, HttpStatusCode statusCode, bool requireResponse)
//        {
//            var client = factory.CreateClient();
//            var response = await client.PostAsJsonAsync(loginUrl, requestDto);

//            Assert.Equal(statusCode, response.StatusCode);

//            if (requireResponse)
//            {
//                var dto = await response.Content.ReadFromJsonAsync<ResponseDto<string>>();

//                Assert.IsType<ResponseDto<string>>(dto);
//                Assert.NotEmpty(dto.Payload);
//            }
//        }
//    }
//}
