using System;
using System.Collections.Generic;
using GameBackend.Enums;
using GameBackend.Models;

namespace GameBackend
{
    public static class Database
    {
        static Database()
        {
            Users = new List<User>
            {
                new User { Id = 1, Username = "user1", Password = "1234"},
                new User { Id = 2, Username = "user2", Password = "1234"},
                new User { Id = 3, Username = "user3", Password = "1234"},
                new User { Id = 4, Username = "user4", Password = "1234"},
            };

            Leaderboards = new List<Leaderboard>
            {
                new Leaderboard
                {
                    GameType = GameType.FirstType,
                    Positions = new List<UserPosition>
                    {
                        new UserPosition
                        {
                            User = Users[0],
                            Score = 50,
                        },
                        new UserPosition
                        {
                            User = Users[1],
                            Score = 100,
                        },
                        new UserPosition
                        {
                            User = Users[2],
                            Score = 150,
                        },
                        new UserPosition
                        {
                            User = Users[3],
                            Score = 200,
                        }
                    }
                },
                new Leaderboard
                {
                    GameType = GameType.Secondtype,
                    Positions = new List<UserPosition>
                    {
                        new UserPosition
                        {
                            User = Users[3],
                            Score = 230,
                        },
                        new UserPosition
                        {
                            User = Users[2],
                            Score = 260,
                        },
                        new UserPosition
                        {
                            User = Users[1],
                            Score = 290,
                        },
                        new UserPosition
                        {
                            User = Users[0],
                            Score = 320,
                        }
                    }
                }
            };

            Matches = new List<Match>
            {
                new Match
                {
                    MatchId = 1,
                    GameType = GameType.FirstType,
                    ConnectedUsers = new List<User>()
                },
                new Match
                {
                    MatchId = 2,
                    GameType = GameType.Secondtype,
                    ConnectedUsers = new List<User>()
                }
            };
        }

        public static List<User> Users { get; }
        public static List<Leaderboard> Leaderboards { get; }
        public static List<Match> Matches { get; private set; }
    }
}
