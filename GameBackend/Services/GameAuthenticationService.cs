using System.Linq;
using GameBackend.Models;
using GameBackend.Services.Abstract;

namespace GameBackend.Services
{
    public class GameAuthenticationService : IGameAuthenticationService
    {
        public User Authenticate(string username, string password)
        {
            return Database.Users.FirstOrDefault(user => user.Username == username && user.Password == password);
        }
    }
}
