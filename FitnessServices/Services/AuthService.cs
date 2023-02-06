using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace FitnessServices.Services;

public static class AuthService
{
    public static bool CheckAuthV2(HttpRequest req)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var auth = req.Headers["Authorization"].ToString();
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = auth.Split(' ').ElementAt(1);

        if (userId == Guid.Empty || string.IsNullOrEmpty(token))
        {
            return false;
        }

        var jwtToken = tokenHandler.ReadJwtToken(token);

        if (jwtToken.Issuer is "https://workout-track.com" or "google.com" or "apple.com")
        {
            return true;
        }

        return false;
    }
}