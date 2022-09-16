namespace FitnessRepository.Models;

public sealed class Users
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastLogin { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }
    public string Password { get; set; }
    public string Sex { get; set; }
}