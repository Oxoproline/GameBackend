using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GameBackend.Constants;
using GameBackend.Models;
using GameBackend.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GameBackend.Infrastracture
{
    public class SimpleAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ITokenService _tokenService;

        public SimpleAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenService tokenService)
            : base(options, logger, encoder, clock)
        {
            _tokenService = tokenService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Path.Value.Contains("match/room"))
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                    return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

                (int UserId, int MatchId)? userMatch;
                try
                {
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    userMatch = _tokenService.VerifyMatchToken(authHeader.Parameter);
                }
                catch
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
                }

                if (userMatch == null)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));

                var claims = new[]
                {
                    new Claim(AuthenticationConstants.UserIdClaim, userMatch?.UserId.ToString()),
                    new Claim(AuthenticationConstants.MatchIdClaim, userMatch?.MatchId.ToString()),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                    return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

                User user;
                try
                {
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    user = _tokenService.VerifyToken(authHeader.Parameter);
                }
                catch
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
                }

                if (user == null)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
        }
    }
}
