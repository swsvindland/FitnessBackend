using Microsoft.AspNetCore.Http;

namespace FitnessServices.Services;

public interface IAuthService
{
    Task<bool> CheckAuth(HttpRequest req);
}