using System;
using System.Net;
using Moq;
using WeeControl.Backend.WebApi;
using WeeControl.Common.FunctionalService.Enums;
using WeeControl.Common.FunctionalService.EssentialContext.Authorization;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using Xunit;

namespace WeeControl.test.WebApi.Test.Functional.Controllers.Essentials.Authorization
{
    public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        //private Mock<IUserDevice> device;

        public RegisterTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void WhenNewUserRegisterWithValidData_ReturnSuccess()
        {
            var mocks = ApplicationMocks.GetMocks(factory.CreateClient(), typeof(RegisterTests).Namespace);

            var response = 
                await new UserOperation(
                    mocks.userDevice.Object, 
                    mocks.userCommunication.Object, 
                    mocks.userStorage.Object).RegisterAsync(RegisterDto.Create("email@email.com", "username", "password"));
            

            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
            Assert.True(response.IsSuccess);
            mocks.userStorage.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));;
        }
    }
}
