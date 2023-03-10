// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using WeeControl.Application.Contexts.Essential.Queries;
// using WeeControl.Domain.Contexts.Essential;
// using Xunit;
//
// namespace WeeControl.Application.Test.Essential.Queries;
//
// public class TerritoriesQueryTests
// {
//     [Fact]
//     public async void WhenDefaultConstructor_ReturnAllTerritories()
//     {
//         using var testHelper = new TestHelper();
//         var handler = await GetHandler(testHelper);
//         
//         var list = await handler.Handle(new TerritoryQuery(), default);
//         
//         Assert.Equal(GetData().Count(), list.Payload.Count());
//     }
//     
//     [Fact]
//     public async void WhenQueringWithInvalidCode_ReturnEmpty()
//     {
//         using var testHelper = new TestHelper();
//         var handler = await GetHandler(testHelper);
//         
//         var list = await handler.Handle(new TerritoryQuery("invalid"), default);
//         
//         Assert.Empty(list.Payload);
//     }
//
//     [Theory]
//     [InlineData("ex", 0)]
//     [InlineData("e0", 1)]
//     [InlineData("m1", 1)]
//     [InlineData("m0", 9)]
//     [InlineData("m2", 5)]
//     [InlineData("m23", 2)]
//     public async void NestingTests1(string code, int count)
//     {
//         using var testHelper = new TestHelper();
//         var handler = await GetHandler(testHelper);
//         
//         var list = await handler.Handle(new TerritoryQuery(code), default);
//         
//         Assert.Equal(count, list.Payload.Count());
//     }
//     
//     [Theory]
//     [InlineData(new[] {"m0","e0"}, 10)]
//     [InlineData(new[] {"m0","m21"}, 9)]
//     [InlineData(new[] {"m2","m21"}, 5)]
//     public async void NestingTests2(string[] codes, int count)
//     {
//         using var testHelper = new TestHelper();
//         var handler = await GetHandler(testHelper);
//         
//         var list = await handler.Handle(new TerritoryQuery(codes), default);
//         
//         Assert.Equal(count, list.Payload.Count());
//     }
//
//     private async Task<TerritoryQuery.GetListOfTerritoriesHandler> GetHandler(TestHelper testHelper)
//     {
//         await testHelper.EssentialDb.Territories.AddRangeAsync(GetData());
//         await testHelper.EssentialDb.SaveChangesAsync(default);
//         return  new TerritoryQuery.GetListOfTerritoriesHandler(
//             testHelper.EssentialDb);
//     }
//
//     private IEnumerable<TerritoryDbo> GetData()
//     {
//         return new List<TerritoryDbo>()
//         {
//             new TerritoryDbo("e0", null, "ccc", "name"),
//             
//             new TerritoryDbo("m0", null, "ccc", "name"),
//             new TerritoryDbo("m1", "m0", "ccc", "name"),
//             TerritoryDbo.Create("m2", "m0", "ccc", "name"),
//             TerritoryDbo.Create("m21", "m2", "ccc", "name"),
//             TerritoryDbo.Create("m22", "m2", "ccc", "name"),
//             TerritoryDbo.Create("m23", "m2", "ccc", "name"),
//             TerritoryDbo.Create("M231", "m23", "ccc", "name"),
//             TerritoryDbo.Create("m3", "m0", "ccc", "name"),
//             TerritoryDbo.Create("m31", "m3", "ccc", "name")
//         };
//     }
// }

