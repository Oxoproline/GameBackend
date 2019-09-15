using System;
using System.Collections.Generic;
using GameBackend.Enums;

namespace GameBackend.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public GameType GameType { get; set; }
        public List<User> ConnectedUsers { get; set; }
    }
}
