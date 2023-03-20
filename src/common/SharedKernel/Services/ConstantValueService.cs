using Newtonsoft.Json;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Models;

namespace WeeControl.Core.SharedKernel.Services;

internal class ConstantValueService : IConstantValue
{
    private static IEnumerable<CountryModel>? _countryModels;

    public IEnumerable<CountryModel> Countries
    {
        get { return _countryModels ??= ReadFromJson(); }
    }

    private static IEnumerable<CountryModel> ReadFromJson()
    {
        var ns = typeof(ConstantValueService).Namespace!.Split(".").Last();
        if (ns is null) throw new NullReferenceException();

        var file = Path.Combine(ns, "Countries.json");

        using var sr = new StreamReader(file);
        var json = sr.ReadToEnd();
        if (string.IsNullOrEmpty(json)) throw new FileNotFoundException();

        var array = JsonConvert.DeserializeObject<IEnumerable<CountryModel>>(json);

        return array!;
    }
}