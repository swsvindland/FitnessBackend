using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository.Models;

public class Users
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastLogin { get; set; }
    public string Email { get; set; }
}