using GameBackend.Models;

namespace GameBackend.Services.Abstract
{
    public interface ITokenService
    {
        string GetToken(User user);
        string GetMatchToken(User user, Match match);

        User VerifyToken(string token);
        (int userId, int matchId)? VerifyMatchToken(string token);
    }
}
