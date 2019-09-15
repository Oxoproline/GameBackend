using System;
using GameBackend.Constants;
using Microsoft.AspNetCore.Authorization;

namespace GameBackend.Infrastracture
{
    public class MatchAuthorizationAttribute : AuthorizeAttribute
    {
        public MatchAuthorizationAttribute()
        {
            Policy = AuthenticationConstants.ValidMatchPolicy;
        }
    }
}
