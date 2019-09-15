using System.Linq;
using System.Security.Claims;
using GameBackend.Enums;
using GameBackend.Models;
using GameBackend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public GameController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        [Route("leaderboards/{gameType}")]
        public IActionResult GetLeaderboard(GameType gameType)
        {
            var leaderboard = Database.Leaderboards
                .FirstOrDefault(x => x.GameType == gameType)
                .Positions
                .OrderBy(x => x.Score)
                .Select(x => new { UserName = x.User.Username, Score = x.Score });

            return Ok(leaderboard);
        }

        [HttpGet]
        [Route("match")]
        public IActionResult GetMatches()
        {
            var matches = Database.Matches.Select(x => new { x.MatchId, x.GameType, NumberOfPlayers = x.ConnectedUsers.Count });

            return Ok(matches);
        }

        [HttpPost]
        [Route("match/{id}/join")]
        public IActionResult JoinMatch(int id)
        {
            User user = null;
            if (int.TryParse(HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                user = Database.Users.FirstOrDefault(x => x.Id == userId);
            }

            if (user is null)
            {
                return BadRequest("Error find authenticated user");
            }

            var match = Database.Matches.FirstOrDefault(x => x.MatchId == id);

            return Ok(_matchService.ConnectToMatch(user.Id, match.MatchId));
        }
    }
}
