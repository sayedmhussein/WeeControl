using WeeControl.Host.WebApiService.Models;

namespace WeeControl.Host.WebApiService.Interfaces;

public interface IConstantValue
{
    IEnumerable<CountryModel> Countries { get; }
}