using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace FitnessServices.Services;

public sealed class AuthService : IAuthService
{
    public bool CheckAuth(HttpRequest req)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var token = req.Headers["Authorization"].ToString().Split().LastOrDefault();

        if (userId == Guid.Empty || string.IsNullOrEmpty(token))
        {
            return false;
        }

        return true;
        // return ValidateToken(token);
    }
    
    private static bool ValidateToken(string authToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        tokenHandler.ValidateToken(authToken, validationParameters, out var validatedToken);
        return validatedToken != null;
    }

    private static TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters()
        {
            ValidateLifetime = false, // Because there is no expiration in the generated token
            ValidateAudience = false, // Because there is no audience in the generated token
            ValidateIssuer = false,   // Because there is no issuer in the generated token
            ValidIssuer = "Sample",
            ValidAudience = "Sample",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET") ?? "")) // The same key as the one that generate the token
        };
    }
}