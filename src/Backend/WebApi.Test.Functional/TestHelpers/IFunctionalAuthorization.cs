using System.Threading.Tasks;

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public interface IFunctionalAuthorization
    {
        Task<string> GetTokenAsync(string username, string password, string device = null);
    }
}
