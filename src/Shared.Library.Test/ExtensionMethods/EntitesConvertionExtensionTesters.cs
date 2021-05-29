
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.ExtensionMethod;
using Xunit;

namespace MySystem.SharedKernel.Test.ExtensionMethods
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
