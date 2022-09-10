using FitnessRepository.Models;

namespace FitnessServices.Models;

public class UserSupplementModel
{
    public long? Id { get; set; }
    public Guid UserId { get; set; }
    public long SupplementId { get; set; }
    public Supplements Supplement { get; set; }
    public string[] Times { get; set; }
}