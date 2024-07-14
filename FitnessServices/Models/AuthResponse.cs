namespace FitnessServices.Models;

public sealed class AuthResponse
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
}