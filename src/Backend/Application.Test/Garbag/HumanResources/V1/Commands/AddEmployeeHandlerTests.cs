//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using Application.Employee.Command.CreateEmployee.V1;
//using Microsoft.Extensions.DependencyInjection;
//using Moq;
//using MySystem.Application.Common.Exceptions;
//using MySystem.Application.Common.Interfaces;
//using MySystem.Application.Employee.Command.AddEmployee.V1;
//using MySystem.Persistence;
//using MySystem.SharedKernel.Entities.Employee.V1Dto;
//using MySystem.SharedKernel.Entities.Public.Constants;
//using Xunit;
//namespace MySystem.Application.Test.Employee.Command.AddEmployee.V1
//{
//    public class AddEmployeeHandlerTests : IDisposable
//    {
//        private IMySystemDbContext dbContext;
//        private EmployeeDto employeeDto;

//        public AddEmployeeHandlerTests()
//        {
//            dbContext = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();

//            employeeDto = new EmployeeDto()
//            {
//                EmployeeTitle = Titles.List[Titles.Title.Mr],
//                FirstName = "Admin",
//                LastName = "Admin",
//                Gender = Genders.List[Genders.Gender.Male],
//                TerritoryId = Guid.NewGuid(),
//                Username = "admin",
//                Password = "admin"
//            };
//        }

//        public void Dispose()
//        {
//            employeeDto = null;
//        }

//        [Fact]
//        public async void WhenAddingNewEmployeeWithCorrectOfficeAndCorrectClaimsAndCorrectClaimTags_ReturnSameEmployee()
//        {
//            var claimType = Claims.Types[Claims.ClaimType.HumanResources];
//            var claimValue = "bla;" + Claims.Tags[Claims.ClaimTag.Add]+ ";bla";
//            //
//            var currentUserMock = new Mock<ICurrentUserInfo>();
//            currentUserMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(claimType, claimValue) });
//            currentUserMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { employeeDto.TerritoryId });

//            AddEmployeeCommand command = new() { DeviceId = "-", Payload = employeeDto };

//            var response = await new AddEmployeeHandler(dbContext, currentUserMock.Object).Handle(command, default);

//            Assert.NotEqual(Guid.Empty, response.Payload.Id);
//        }

//        [Fact]
//        public async void WhenAddingNewEmployeeInvalidDto_ThrowValidationException()
//        {
//            employeeDto.FirstName = null;
//            var claimType = Claims.Types[Claims.ClaimType.HumanResources];
//            var claimValue = "bla;" + Claims.Tags[Claims.ClaimTag.Add] + ";bla";
//            //
//            var currentUserMock = new Mock<ICurrentUserInfo>();
//            currentUserMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(claimType, claimValue) });
//            currentUserMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { employeeDto.TerritoryId });

//            AddEmployeeCommand command = new() { DeviceId = "-", Payload = employeeDto };

//            await Assert.ThrowsAsync<ValidationException>(async () => await new AddEmployeeHandler(dbContext, currentUserMock.Object).Handle(command, default));
//        }

//        [Fact]
//        public async void WhenAddingNewEmployeeWithDifferentOfficeAndCorrectClaimsAndCorrectClaimTags_ThrowNotAllowedException()
//        {
//            var claimType = Claims.Types[Claims.ClaimType.HumanResources];
//            var claimValue = "bla;" + Claims.Tags[Claims.ClaimTag.Add] + ";bla";
//            //
//            var currentUserMock = new Mock<ICurrentUserInfo>();
//            currentUserMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(claimType, claimValue) });
//            currentUserMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { Guid.NewGuid()});

//            AddEmployeeCommand command = new() { DeviceId = "-", Payload = employeeDto };

//            await Assert.ThrowsAsync<NotAllowedException> (async () => await new AddEmployeeHandler(dbContext, currentUserMock.Object).Handle(command, default));
//        }

//        [Fact]
//        public async void WhenAddingNewEmployeeWithCorrectOfficeAndInvalidClaimsAndCorrentClaimTags_ThrowNotAllowedException()
//        {
//            var claimType = Claims.Types[Claims.ClaimType.Office];
//            var claimValue = "bla;" + Claims.Tags[Claims.ClaimTag.Add] + ";bla";
//            //
//            var currentUserMock = new Mock<ICurrentUserInfo>();
//            currentUserMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(claimType, claimValue) });
//            currentUserMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { employeeDto.TerritoryId });

//            AddEmployeeCommand command = new() { DeviceId = "-", Payload = employeeDto };

//            await Assert.ThrowsAsync<NotAllowedException>(async () => await new AddEmployeeHandler(dbContext, currentUserMock.Object).Handle(command, default));
//        }

//        [Fact]
//        public async void WhenAddingNewEmployeeWithCorrectOfficeAndCorrectClaimsAndInvalidClaimTags_ThrowNotAllowedException()
//        {
//            var claimType = Claims.Types[Claims.ClaimType.HumanResources];
//            var claimValue = "bla;" + Claims.Tags[Claims.ClaimTag.Edit] + ";bla";
//            //
//            var currentUserMock = new Mock<ICurrentUserInfo>();
//            currentUserMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(claimType, claimValue) });
//            currentUserMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { employeeDto.TerritoryId });

//            AddEmployeeCommand command = new() { DeviceId = "-", Payload = employeeDto };

//            await Assert.ThrowsAsync<NotAllowedException>(async () => await new AddEmployeeHandler(dbContext, currentUserMock.Object).Handle(command, default));
//        }
//    }
//}
