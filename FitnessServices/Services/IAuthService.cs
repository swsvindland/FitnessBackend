using Microsoft.AspNetCore.Http;

namespace FitnessServices.Services;

public interface IAuthService
{
    bool CheckAuth(HttpRequest req);
}