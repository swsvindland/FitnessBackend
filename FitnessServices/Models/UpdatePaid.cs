using FitnessRepository.Models;

namespace FitnessServices.Models;

public class UpdatePaid
{
    public bool Paid { get; set; }
    public DateTime? PaidUntil { get; set; }
}