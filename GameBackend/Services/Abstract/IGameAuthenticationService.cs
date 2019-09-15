using GameBackend.Models;

namespace GameBackend.Services.Abstract
{
    public interface IGameAuthenticationService
    {
        User Authenticate(string username, string password);
    }
}
