using WeeControl.User.UserApplication.Models;

namespace WeeControl.User.UserApplication.Interfaces;

public interface IPersistedLists
{
    IEnumerable<CountryModel> Countries { get; }
}