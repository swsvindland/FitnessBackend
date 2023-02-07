using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace FitnessServices.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserService _userService;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> CheckAuth(HttpRequest req)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var token = req.Headers["Authorization"].ToString().Split().LastOrDefault();
        var tokenHandler = new JwtSecurityTokenHandler();

        if (userId == Guid.Empty || string.IsNullOrEmpty(token))
        {
            return false;
        }

        var jwtToken = tokenHandler.ReadJwtToken(token);
        var user = await _userService.GetUserById(userId);
        
        return string.Equals(
            jwtToken.Issuer != "https://workout-track.com"
                ? jwtToken.Claims.FirstOrDefault(e => e.Type == "email")?.Value
                : jwtToken.Subject, user?.Email, StringComparison.CurrentCultureIgnoreCase);
    }
}