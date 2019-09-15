using System;
using System.Collections.Generic;
using GameBackend.Enums;

namespace GameBackend.Models
{
    public class Leaderboard
    {
        public GameType GameType { get; set; }

        public List<UserPosition> Positions { get; set; }
    }
}
