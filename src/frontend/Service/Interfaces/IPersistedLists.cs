using WeeControl.Frontend.AppService.Contexts.Essential.Models;

namespace WeeControl.Frontend.AppService.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}