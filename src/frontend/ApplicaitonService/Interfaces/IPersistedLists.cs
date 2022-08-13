using WeeControl.Frontend.ApplicationService.Anonymous.Models;

namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}