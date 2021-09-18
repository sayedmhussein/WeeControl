using System.Threading.Tasks;

namespace WeeControl.Frontend.CommonLib.Interfaces
{
    public interface IAuthenticationRefresh
    {
        public Task AuthenticationRefreshedAsync();
    }
}