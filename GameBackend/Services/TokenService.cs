using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using GameBackend.Models;
using GameBackend.Services.Abstract;

namespace GameBackend.Services
{
    public class TokenService : ITokenService
    {
        private readonly RSACryptoServiceProvider _rsaCSP;
        private readonly RSAParameters _rsaPrivateParams;

        public TokenService()
        {
            _rsaCSP = new RSACryptoServiceProvider();

            _rsaPrivateParams = _rsaCSP.ExportParameters(true);
        }

        public string GetMatchToken(User user, Models.Match match)
        {
            return GenerateToken($"{user.Id}-{match.MatchId}");
        }

        public string GetToken(User user)
        {
            return GenerateToken(user.Username);
        }

        public User VerifyToken(string token)
        {
            var userName = VerifyTokenString(token);

            return Database.Users.FirstOrDefault(x => x.Username == userName);
        }

        public (int userId, int matchId)? VerifyMatchToken(string token)
        {
            var userMatch = VerifyTokenString(token).Split('-');

            if (int.TryParse(userMatch[0], out int userId) && int.TryParse(userMatch[0], out int matchId))
            {
                return (userId, matchId);
            }

            return null;
        }

        private string GenerateToken(string inputString)
        {
            _rsaCSP.ImportParameters(_rsaPrivateParams);
            var bytes = _rsaCSP.Encrypt(Encoding.UTF8.GetBytes(inputString), false);

            return Convert.ToBase64String(bytes);
        }

        private string VerifyTokenString(string token)
        {
            var encryptedBytes = Convert.FromBase64String(token);

            _rsaCSP.ImportParameters(_rsaPrivateParams);
            var decryptedBytes = _rsaCSP.Decrypt(encryptedBytes, false);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
