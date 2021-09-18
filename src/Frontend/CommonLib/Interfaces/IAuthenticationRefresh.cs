using System.Threading.Tasks;

namespace WeeControl.Frontend.CommonLib.Interfaces
{
    public interface IAuthenticationRefresh : IAuthenticationBase
    {
        public Task AuthenticationRefreshedAsync();
    }
}