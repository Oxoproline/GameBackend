using System.Linq;
using GameBackend.Services.Abstract;

namespace GameBackend.Services
{
    public class MatchService : IMatchService
    {
        private readonly ITokenService _tokenService;

        public MatchService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public string ConnectToMatch(int userId, int matchId)
        {
            var user = Database.Users.FirstOrDefault(x => x.Id == userId);
            var match = Database.Matches.FirstOrDefault(x => x.MatchId == matchId);

            if (!match.ConnectedUsers.Contains(user))
            {
                match.ConnectedUsers.Add(user);
            }

            return _tokenService.GetMatchToken(user, match);
        }
    }
}
