using WeeControl.Frontend.ApplicationService.Contexts.Anonymous.Models;

namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}