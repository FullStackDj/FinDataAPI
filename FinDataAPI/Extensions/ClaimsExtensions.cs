using System.Security.Claims;

namespace FinDataAPI.Extensions;

public static class ClaimsExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        var claim = user.Claims
            .SingleOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");

        return claim?.Value;
    }
}