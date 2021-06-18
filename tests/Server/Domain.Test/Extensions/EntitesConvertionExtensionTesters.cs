using WeeControl.Server.Domain.BasicDbos.Territory;
using WeeControl.Server.Domain.Extensions;
using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;
using Xunit;

namespace WeeControl.Server.Domain.Test.Extensions
{
    public class EntitesConvertionExtensionTesters
    {
        [Fact]
        public void ToDboTesterOnTerritories()
        {
            var name = "Territory Name";
            var country = "EGP";
            var dto = new TerritoryDto() { Name = name, CountryId = country };
            var dbo = dto.ToDbo<TerritoryDto, TerritoryDbo>();

            Assert.Equal(name, dbo.Name);
            Assert.Equal(country, dbo.CountryId);
        }

        [Fact]
        public void ToDtoTesterOnTerritories()
        {
            var name = "Territory Name";
            var country = "EGP";
            var dbo = new TerritoryDbo() { Name = name, CountryId = country };

            var dto = dbo.ToDto<TerritoryDbo, TerritoryDto>();

            Assert.Equal(name, dto.Name);
            Assert.Equal(country, dto.CountryId);
        }
    }
}
