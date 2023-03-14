using Newtonsoft.Json;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.Interfaces;
using WeeControl.Host.WebApiService.Models;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class ConstantValueService : IConstantValue
{
    private static IEnumerable<CountryModel>? _countryModels;

    public IEnumerable<CountryModel> Countries
    {
        get { return _countryModels ??= ReadFromJson(); }
    }

    private static IEnumerable<CountryModel> ReadFromJson()
    {
        var ns = typeof(ApplicationPages).Namespace!.Split(".").Last();
        if (ns is null) throw new NullReferenceException();

        var file = Path.Combine(ns, "Countries.json");

        using var sr = new StreamReader(file);
        var json = sr.ReadToEnd();
        if (string.IsNullOrEmpty(json)) throw new FileNotFoundException();

        var array = JsonConvert.DeserializeObject<IEnumerable<CountryModel>>(json);

        return array!;
    }
}