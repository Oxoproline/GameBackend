using System;
using System.Numerics;
using System.Threading.Tasks;
using GameBackend.Infrastracture;
using GameBackend.Models;
using Microsoft.AspNetCore.SignalR;

namespace GameBackend
{
    [MatchAuthorization]
    public class GameHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        [MatchAuthorization]
        public void Fire()
        {

        }

        [MatchAuthorization]
        public void Move(Vector3 vector)
        {

        }

        [MatchAuthorization]
        public User[] GetPlayers()
        {
            return Database.Users.ToArray();
        }
    }
}
