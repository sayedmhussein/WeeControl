using MySystem.Domain.EntityDbo.Territory;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.EntityV1Dtos.Territory;
using Xunit;

namespace MySystem.Domain.Test.Extensions
{
    public class EntitesConvertionExtensionTesters
    {
        [Fact]
        public void ToDboTesterOnBuilding()
        {
            var name = "BuildingName__";
            var dto = new TerritoryDto() { Name = name };
            var dbo = dto.ToDbo<TerritoryDto, TerritoryDbo>();

            Assert.Equal(name, dbo.Name);
        }

        [Fact]
        public void ToDtoTesterOnBuilding()
        {
            var name = "BuildingName__";
            var dbo = new TerritoryDbo() { Name = name };

            var dto = dbo.ToDto<TerritoryDbo, TerritoryDto>();

            Assert.Equal(name, dto.Name);
        }
    }
}
