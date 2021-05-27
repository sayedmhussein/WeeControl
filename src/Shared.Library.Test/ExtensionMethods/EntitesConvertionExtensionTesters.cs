using MySystem.Shared.Library.Dbo.Entity;
using MySystem.Shared.Library.Dto.EntityV1;
using MySystem.Shared.Library.ExtensionMethod;
using Xunit;

namespace MySystem.Shared.Library.Test.ExtensionMethods
{
    public class EntitesConvertionExtensionTesters
    {
        [Fact]
        public void ToDboTesterOnBuilding()
        {
            var name = "BuildingName__";
            var dto = new BuildingDto() { BuildingName = name };

            var dbo = dto.ToDbo<BuildingDto, BuildingDbo>();

            Assert.Equal(name, dbo.BuildingName);
        }

        [Fact]
        public void ToDtoTesterOnBuilding()
        {
            var name = "BuildingName__";
            var dbo = new BuildingDbo() { BuildingName = name };

            var dto = dbo.ToDto<BuildingDto, BuildingDbo>();

            Assert.Equal(name, dto.BuildingName);
        }
    }
}
