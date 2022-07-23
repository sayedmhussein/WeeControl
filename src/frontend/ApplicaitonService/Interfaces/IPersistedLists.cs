using WeeControl.Frontend.ApplicationService.Essential.Models;

namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}