using WeeControl.Core.SharedKernel.Models;

namespace WeeControl.Core.SharedKernel.Interfaces;

public interface IConstantValue
{
    IEnumerable<CountryModel> Countries { get; }
}