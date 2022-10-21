using Microsoft.AspNetCore.Http;

namespace FitnessServices.Services;

public class AuthService: IAuthService
{
    private readonly IUserService _userService;
    
    public AuthService(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<bool> CheckAuth(HttpRequest req)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var token = req.Query["token"];
        
        if (userId == Guid.Empty || string.IsNullOrEmpty(token))
        {
            return false;
        }

        var currentToken = await _userService.GetTokenByUserId(userId);

        if (currentToken == null)
        {
            return false;
        }

        return currentToken.Token == token.ToString();
    }
}