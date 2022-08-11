using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Application.Test.Essential.Queries;

public class TerritoryVerifyQueryTests
{
    [Fact]
    public async void WhenExist()
    {
        using var testHelper = new TestHelper();
        testHelper.EssentialDb.Territories
            .Add(TerritoryDbo.Create("code", null, "YYY", "xOXo"));
        testHelper.EssentialDb.Users
            .Add(testHelper
                .GetUserDboWithEncryptedPassword("username", "password", "code"));
        testHelper.EssentialDb.SaveChanges();
        
        testHelper.EssentialDb.UserSessions.Add(UserSessionDbo.Create(testHelper.EssentialDb.Users.First().UserId, "device"));
        testHelper.EssentialDb.SaveChanges();

        testHelper.CurrentUserInfoMock.Setup(x => x.SessionId)
            .Returns(testHelper.EssentialDb.UserSessions.First().SessionId);

        testHelper.MediatorMock
            .Setup(x => x.Send(It.IsAny<TerritoryQuery>(), default))
            .ReturnsAsync(ResponseDto
                .Create<IEnumerable<TerritoryDto>>(
                    new List<TerritoryDto>()
                    {
                        new TerritoryDto()
                        {
                            TerritoryCode = "code", CountryCode = "cod", TerritoryName = "name"
                        }
                    }));

        var handler = await GetHandler(testHelper);
        
        Assert.True(await handler.Handle(new TerritoryVerifyQuery("code"), default));
    }
    
    private async Task<TerritoryVerifyQuery.VerifyUserTerritoryHandler> GetHandler(TestHelper testHelper)
    {
        await testHelper.EssentialDb.SaveChangesAsync(default);
        return new TerritoryVerifyQuery.VerifyUserTerritoryHandler(
            testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object, testHelper.MediatorMock.Object);
    }
}