using WeeControl.Frontend.AppService.Models;

namespace WeeControl.Frontend.AppService.Internals.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}