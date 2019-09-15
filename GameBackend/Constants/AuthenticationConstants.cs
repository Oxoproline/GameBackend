using System;
namespace GameBackend.Constants
{
    public class AuthenticationConstants
    {
        public const string Scheme = "CustomAuthScheme";

        public const string UserIdClaim = nameof(UserIdClaim);
        public const string MatchIdClaim = nameof(MatchIdClaim);

        public const string ValidMatchPolicy = nameof(ValidMatchPolicy);
    }
}
