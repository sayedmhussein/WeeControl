using System.Collections.Immutable;
using WeeControl.Applications.BaseLib.Enumerators;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IEmployeeLibValues
    {
        ImmutableDictionary<ApplicationPageEnum, string> ApplicationPage { get; }
    }
}
