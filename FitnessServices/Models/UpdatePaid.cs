namespace FitnessServices.Models;

public sealed class UpdatePaid
{
    public bool Paid { get; set; }
    public DateTime? PaidUntil { get; set; }
}