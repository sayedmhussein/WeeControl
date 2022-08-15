// using System.Threading.Tasks;
// using MediatR;
// using WeeControl.Application.Contexts.Essential.Queries;
// using WeeControl.Application.Exceptions;
// using Xunit;
//
// namespace WeeControl.Application.Test.Essential.Queries;
//
// public class UserDuplicationQueryTests
// {
//     [Fact]
//     public async void TestForSuccess()
//     {
//         using var testHelper = new TestHelper();
//
//         var handler = await GetHandler(testHelper);
//
//         var result = await handler.Handle(new UserDuplicationQuery("username1", "email@email.com", "+33"), default);
//         Assert.Equal(Unit.Value, result);
//     }
//     
//     [Theory]
//     [InlineData("username", "", "")]
//     [InlineData("", "user@email.com", "")]
//     [InlineData("", "", "+2022")]
//     public async void TestsForFailures(string username, string email, string mobileNo)
//     {
//         using var testHelper = new TestHelper();
//         
//         await Assert.ThrowsAsync<ConflictFailureException>(async () => await 
//             (await GetHandler(testHelper)).Handle(
//                 new UserDuplicationQuery(username:username, email: email, mobile: mobileNo), 
//                 default));
//     }
//     
//     private async Task<UserDuplicationQuery.UserDuplicationHandler> GetHandler(TestHelper testHelper)
//     {
//         var user = testHelper.GetUserDboWithEncryptedPassword("username", "password");
//         user.Email = "user@email.com";
//         user.MobileNo = "+2022";
//         await testHelper.EssentialDb.Users
//             .AddAsync(user);
//         await testHelper.EssentialDb.SaveChangesAsync(default);
//         return new UserDuplicationQuery.UserDuplicationHandler(testHelper.EssentialDb);
//     }
// }