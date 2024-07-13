using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Models;

public sealed class UserFood
{
    public long Id { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    [ForeignKey("FoodId")]
    public long FoodId { get; set; }
    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Food? Food { get; set; }
    [ForeignKey("ServingId")]
    public long ServingId { get; set; }
    public FoodServings? Serving { get; set; }
    public float ServingAmount { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}