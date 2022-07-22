using WeeControl.Frontend.ApplicationService.Essential.Territory;

namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}