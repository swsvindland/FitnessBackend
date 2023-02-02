using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace FitnessServices.Services;

public sealed class AuthService: IAuthService
{
    private readonly IUserService _userService;
    
    public AuthService(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<bool> CheckAuth(HttpRequest req)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var token = req.Query["token"].ToString();
        
        if (userId == Guid.Empty || string.IsNullOrEmpty(token))
        {
            return false;
        }
        
        var currentToken = await _userService.GetToken(userId, token);

        if (currentToken == null)
        {
            return await CheckAuthV2(req);
        }

        return currentToken.Token == token;
    }
    
    private async Task<bool> CheckAuthV2(HttpRequest req)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var token = req.Query["token"].ToString();
        var tokenHandler = new JwtSecurityTokenHandler();

        if (userId == Guid.Empty || string.IsNullOrEmpty(token))
        {
            return false;
        }

        var jwtToken = tokenHandler.ReadJwtToken(token);
        var user = await _userService.GetUserById(userId);

        if (jwtToken.Issuer != "https://workout-track.com")
        {
            return jwtToken.Claims.FirstOrDefault(e => e.Type == "email")?.Value == user?.Email;
        }

        return jwtToken.Subject == user?.Email;
    }
}