using WeeControl.Frontend.Service.Contexts.Essential.Models;

namespace WeeControl.Frontend.Service.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}