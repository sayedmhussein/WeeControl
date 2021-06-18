using WeeControl.Server.Domain.EntityDbo.Territory;
using WeeControl.Server.Domain.Extensions;
using WeeControl.SharedKernel.CommonSchemas.Territory.DtosV1;
using Xunit;

namespace WeeControl.Server.Domain.Test.Extensions
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
