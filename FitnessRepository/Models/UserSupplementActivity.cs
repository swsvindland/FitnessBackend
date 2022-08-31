using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository.Models;

public class UserSupplementActivity
{
    public long? Id { get; set; }
    public DateTime Updated { get; set; }
    public long UserSupplementId { get; set; }
    public Guid UserId { get; set; }
    public UserSupplement? UserSupplement { get; set; }
    public SupplementTimes Time { get; set; }
}