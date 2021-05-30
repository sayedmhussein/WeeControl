using MySystem.Domain.EntityDbo.UnitSchema;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.Dto.V1;
using Xunit;

namespace MySystem.Domain.Test.Extensions
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
