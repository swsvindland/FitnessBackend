using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository.Models;

public sealed class Users
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastLogin { get; set; }
    public string Email { get; set; }
    public string? Salt { get; set; }
    public string? Password { get; set; }
    [Column(TypeName = "varchar(255)")]
    public Sex Sex { get; set; }
    [Column(TypeName = "varchar(255)")]
    public UserRole UserRole { get; set; }
    [Column(TypeName = "varchar(255)")]
    public UserUnits Unit { get; set; }
    public bool Paid { get; set; }
    public DateTime? PaidUntil { get; set; }
    public int LoginCount { get; set; }
}