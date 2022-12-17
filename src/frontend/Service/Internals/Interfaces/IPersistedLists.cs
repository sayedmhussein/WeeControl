using WeeControl.Frontend.AppService.Contexts.Temporary.Models;

namespace WeeControl.Frontend.AppService.Internals.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}